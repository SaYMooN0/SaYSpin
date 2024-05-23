using SaYSpin.src.gameplay_parts.inventory_related;
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
using SaYSpin.src.game_saving;
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

        internal GameFlowController(
            Dictionary<string, Func<TileItem>> allTileItemsConstructors, Dictionary<string, Func<Relic>> allRelicsConstructors,
            Difficulty difficulty, SlotMachine slotMachine, Inventory inventory, short spinsLeft, int currentStage, int coinsCount, int coinsNeededToCompleteTheStage, RunProgressController runProgressController, StatsTracker statsTracker, ShopController shop, bool areCheatsEnabled, BeforeNewStageDialogDelegate showBeforeStageDialog)
        {
            this.allTileItemsConstructors = new ReadOnlyDictionary<string, Func<TileItem>>(allTileItemsConstructors);
            AllTileItemsCollection = allTileItemsConstructors.Select(ti => ti.Value()).ToArray();
            this.allRelicsConstructors = new ReadOnlyDictionary<string, Func<Relic>>(allRelicsConstructors);
            AllRelicsCollection = allRelicsConstructors.Select(relic => relic.Value()).ToArray();
            Difficulty = difficulty;
            SlotMachine = slotMachine;
            Inventory = inventory;
            SpinsLeft = spinsLeft;
            CurrentStage = currentStage;
            CoinsCount = coinsCount;
            CoinsNeededToCompleteTheStage = coinsNeededToCompleteTheStage;
            RunProgressController = runProgressController;
            StatsTracker = statsTracker;
            Shop = shop;
            AreCheatsEnabled = areCheatsEnabled;
            ShowBeforeStageDialog = showBeforeStageDialog;
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
        }
        internal GameFlowController(
          StatsTracker stats,
          Difficulty difficulty,
          Dictionary<string, Func<TileItem>> accessibleTileItems,
          Dictionary<string, Func<Relic>> accessibleRelics,
          ISpecialMerchant[] possibleSpecialMerchants,
          BeforeNewStageDialogDelegate beforeStageDialogShowingDelegate,
          bool areCheatsEnabled
            ) : this(
              accessibleTileItems, accessibleRelics,
              difficulty,
              null, null, 0, 0, 0, 0, 
              new RunProgressController(difficulty),
              stats,
              new ShopController(possibleSpecialMerchants),
              areCheatsEnabled,
              beforeStageDialogShowingDelegate

              )
        { }

        internal void Start(GameStarterKit chosenStarterKit)
        {
            Inventory = new(chosenStarterKit.TileItems, chosenStarterKit.Relics, chosenStarterKit.TokensCollection, chosenStarterKit.DiamondsCount);
            Inventory.Tokens.TokensCountChanged += () => OnInventoryChanged?.Invoke();
            SlotMachine = new(Inventory.TileItems, 3, 3);

            CurrentStage = 0;
            StartNewStage();
        }
        internal StageCompletionResult CompleteStage()
        {
            int extraCoins = CoinsCount - CoinsNeededToCompleteTheStage;
            int diamondsFromCoins = (int)(extraCoins * StatsTracker.AfterStageCoinsToDiamondsCoefficient / (CurrentStage * 2.2 + 4));
            int diamondsFromSpins = (int)((CurrentStage / 2 + 1) * SpinsLeft);

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
            SavingController.SaveGame(this);
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
               (Math.Pow(CurrentStage * 1.25 + +1, 2.3) * 2 * StatsTracker.CoinsNeededToCompleteStage) + 10;

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
