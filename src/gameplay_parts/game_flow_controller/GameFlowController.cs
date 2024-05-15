﻿using SaYSpin.src.gameplay_parts.inventory_related;
using SaYSpin.src.secondary_classes;
using SaYSpin.src.extension_classes;
using SaYSpin.src.inventory_items.tile_items;
using SaYSpin.src.inventory_items.relics;
using SaYSpin.src.inventory_items.relics.relic_effects;
using SaYSpin.src.inventory_items;
using SaYSpin.src.gameplay_parts.inventory_related.tokens;
using System.Collections.ObjectModel;
using SaYSpin.src.gameplay_parts.shop;
using SaYSpin.src.gameplay_parts.run_progress;
namespace SaYSpin.src.gameplay_parts.game_flow_controller
{
    public partial class GameFlowController
    {
        private ReadOnlyDictionary<string, Func<TileItem>> allTileItemsConstructors { get; init; }
        public TileItem[] AllTileItemsCollection { get; init; }
        private ReadOnlyDictionary<string, Func<Relic>> allRelicsConstructors { get; init; }
        public Relic[] AllRelicsCollection { get; init; }
        public Difficulty Difficulty { get; init; }
        public SlotMachine SlotMachine { get; private set; }
        public Inventory Inventory { get; private set; }
        public short SpinsLeft { get; private set; }
        public int CurrentStage { get; private set; }
        public int CoinsCount { get; private set; }
        public int CoinsNeededToCompleteTheStage { get; private set; }
        public RunProgressController RunProgressController { get; init; }
        public StatsTracker StatsTracker { get; init; }
        public ShopController Shop { get; init; }
        public bool AreCheatsEnabled { get; init; }
        private BeforeNewStageDialogDelegate ShowBeforeStageDialog { get; init; }
        public GameFlowController(
            StatsTracker stats,
            Difficulty difficulty,
            Dictionary<string, Func<TileItem>> accessibleTileItems,
            Dictionary<string, Func<Relic>> accessibleRelics,
            ISpecialMerchant[] possibleSpecialMerchants,
            BeforeNewStageDialogDelegate beforeStageDialogShowingDelegate,
            bool areCheatsEnabled)
        {

            AreCheatsEnabled = areCheatsEnabled;
            Difficulty = difficulty;


            allTileItemsConstructors = new ReadOnlyDictionary<string, Func<TileItem>>(accessibleTileItems);
            AllTileItemsCollection = accessibleTileItems.Select(ti => ti.Value()).ToArray();

            allRelicsConstructors = new ReadOnlyDictionary<string, Func<Relic>>(accessibleRelics);
            AllRelicsCollection = accessibleRelics.Select(r => r.Value()).ToArray();

            RunProgressController = new(difficulty);
            StatsTracker = stats;
            Shop = new(possibleSpecialMerchants);

            ShowBeforeStageDialog = beforeStageDialogShowingDelegate;
            this.OnInventoryItemAdded += (item) =>
            {
                OnInventoryChanged?.Invoke();
                if (item.IsUnique)
                    SetPossibleInventoryItemsReinitNeeded(item);
            };
            this.OnInventoryItemRemoved += (item) =>
            {
                OnInventoryChanged?.Invoke();
                if (item.IsUnique)
                    SetPossibleInventoryItemsReinitNeeded(item);
            };
            this.OnTokenUsed += (token) =>
            {
                OnInventoryChanged?.Invoke();
            };

        }
        internal void Start(GameStarterKit chosenStarterKit)
        {
            Inventory = new(chosenStarterKit.TileItems, chosenStarterKit.Relics, chosenStarterKit.TokensCollection, chosenStarterKit.DiamondsCount);
            SlotMachine = new(Inventory.TileItems, 3, 3);

            CurrentStage = 0;
            StartNewStage();
        }
        internal StageCompletionResult CompleteStage()
        {
            int extraCoins = CoinsCount - CoinsNeededToCompleteTheStage;
            int diamondsFromCoins = (int)(extraCoins / (CurrentStage + 4) * 1.25 * StatsTracker.AfterStageCoinsToDiamondsCoefficient);
            int diamondsFromSpins = (int)((CurrentStage + 4) / 4.5 * SpinsLeft);

            var rewards = this.GatherAllAfterStageRewards();

            return new(
                CurrentStage,
                CoinsNeededToCompleteTheStage,
                CoinsCount,
                extraCoins,
                SpinsLeft,
                diamondsFromCoins,
                diamondsFromSpins,
                diamondsFromCoins + diamondsFromSpins,
                rewards);
        }
        internal void StartNewStage()
        {
            this.UpdateStatsIfNeeded();

            CurrentStage += 1;
            SpinsLeft = StatsTracker.StageSpinsCount;
            CoinsCount = 0;

            CoinsNeededToCompleteTheStage = this.CalculateCoinsNeededForStage();

            OnNewStageStarted?.Invoke(CurrentStage);
            foreach (var effect in Inventory.Relics.SelectMany(r => r.Effects.OfType<OnStageStartedRelicEffect>()))
            {
                effect.PerformOnStageStartedAction(this);
            }
            EveryStageShopUpdate();

            ShowBeforeStageDialog(RunProgressController.GetNewStageActionsGroup(CurrentStage));
        }
        public void SpinSlotMachine()
        {
            SpinsLeft -= 1;


            TileItemsPicker itemsPicker = new(SlotMachine.TotalSlots);
            itemsPicker.PickItemsFrom(Inventory.TileItems);
            SlotMachine.UpdateItems(itemsPicker);

            HandleTileItemsWithAreaScanningEffects();

            IEnumerable<CoinsCalculationRelicEffect> relicEffects = this.GatherCoinsCalculationRelicEffects();
            int income = SlotMachine.CalculateCoinValue(relicEffects);
            AddCoins(income);

            HandleAfterSpinRelicEffects();
            HandleTransformationEffects();
            HandleTileItemsWithAbsorbingEffects();
            ClearTileItemsMarkers();

            this.UpdateStatsIfNeeded();
        }
        public void AddCoins(int count) =>
            CoinsCount += count;
        internal void SetCurrentCoinsCount(int value) =>
           CoinsCount = value;
        public bool UseToken(TokenType token)
        {
            if (Inventory.Tokens.TryUseToken(token))
            {
                TriggerOnTokenUsedEffects(token);
                OnTokenUsed?.Invoke(token);
                return true;
            }
            return false;
        }

        public void RefreshShopWithToken()
        {
            if (UseToken(TokenType.ShopRefresh))
                UpdateShopItems();
        }
        private void EveryStageShopUpdate()
        {
            UpdateShopItems();
            if (CurrentStage % 5 == 0)
                Shop.UpdateSpecialMerchant();
        }
        private void ClearTileItemsMarkers() =>
           Inventory.TileItems.ForEach(ti => ti.ClearMarkers());
        private int CalculateCoinsNeededForStage() => (int)
               (Math.Pow(CurrentStage * 1.65 + 2, 1.85) * Difficulty.NeededCoinsMultiplier * 2.2 * StatsTracker.CoinsNeededToCompleteStage) - 10;

        public void BuyItem(ItemForSale item)
        {
            Inventory.ChangeDiamondsCount(d => d - item.Price);
            Shop.ItemBought(item);
            if (item.Item is Relic r)
                AddRelicToInventory(r);
            else if (item.Item is TileItem ti)
                AddTileItemToInventory(ti);
        }

        public event Action<int> OnNewStageStarted;

        public delegate Task BeforeNewStageDialogDelegate(BeforeStageActionsGroup actionsGroup);

        public event Action<TileItem> OnTileItemDestruction;
        public event Action<BaseInventoryItem> OnInventoryItemAdded;
        public event Action<BaseInventoryItem> OnInventoryItemRemoved;

        public event Action<TokenType> OnTokenUsed;
        public event Action OnInventoryChanged;


    }
}
