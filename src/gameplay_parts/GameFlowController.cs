using SaYSpin.src.gameplay_parts.inventory_related;
using SaYSpin.src.secondary_classes;
using SaYSpin.src.extension_classes;
using SaYSpin.src.inventory_items.tile_items;
using SaYSpin.src.inventory_items.relics;
using SaYSpin.src.inventory_items.relics.relic_effects;
using SaYSpin.src.inventory_items.tile_items.tile_item_effects;
using SaYSpin.src.inventory_items;
using SaYSpin.src.gameplay_parts.inventory_related.tokens;
namespace SaYSpin.src.gameplay_parts
{
    public class GameFlowController
    {
        private Dictionary<string, Func<TileItem>> _tileItemsCollection { get; init; }
        public List<TileItem> TileItems { get; init; }
        private Dictionary<string, Func<Relic>> _relicsCollection { get; init; }
        public List<Relic> Relics { get; init; }
        public Difficulty Difficulty { get; init; }
        public SlotMachine SlotMachine { get; private set; }
        public Inventory Inventory { get; private set; }
        public short SpinsLeft { get; private set; }
        public int CurrentStage { get; private set; }
        public int CoinsCount { get; private set; }
        public int CoinsNeededToCompleteTheStage { get; private set; }
        public StatsTracker StatsTracker { get; init; }
        public bool AreCheatsEnabled { get; init; }

        public GameFlowController(
            StatsTracker stats,
            Difficulty difficulty,
            Dictionary<string, Func<TileItem>> accessibleTileItems,
            Dictionary<string, Func<Relic>> accessibleRelics,
            bool areCheatsEnabled)
        {
            StatsTracker = stats;
            Difficulty = difficulty;

            _tileItemsCollection = accessibleTileItems;
            TileItems = accessibleTileItems.Select(ti => ti.Value()).ToList();

            _relicsCollection = accessibleRelics;
            Relics = accessibleRelics.Select(r => r.Value()).ToList();

            AreCheatsEnabled = areCheatsEnabled;
        }
        public void Start(GameStarterKit chosenStarterKit)
        {
            Inventory = new(chosenStarterKit.TileItems, chosenStarterKit.Relics, chosenStarterKit.TokensCollection, chosenStarterKit.DiamondsCount);
            SlotMachine = new(Inventory.TileItems, 3, 3);

            CurrentStage = 0;
            StartNewStage();
        }
        public StageCompletionResult CompleteStage()
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
        public void StartNewStage()
        {
            this.UpdateStatsIfNeeded();

            CurrentStage += 1;
            SpinsLeft = StatsTracker.StageSpinsCount;
            CoinsCount = 0;

            OnNewStageStarted?.Invoke(CurrentStage);



            CoinsNeededToCompleteTheStage = this.CalculateCoinsNeededForStage();

            foreach (var effect in Inventory.Relics.SelectMany(r => r.Effects.OfType<OnStageStartedRelicEffect>()))
            {
                effect.PerformOnStageStartedAction(this);
            }

        }
        public void SpinSlotMachine()
        {
            TileItemsPicker itemsPicker = new(SlotMachine.TotalSlots);
            itemsPicker.PickItemsFrom(Inventory.TileItems);
            SlotMachine.UpdateItems(itemsPicker);

            IEnumerable<CoinsCalculationRelicEffect> relicEffects = this.GatherCoinsCalculationRelicEffects();

            int income = SlotMachine.CalculateCoinValue(relicEffects);
            AddCoins(income);
            SpinsLeft -= 1;
            this.HandleAfterSpinRelicEffects();
            this.HandleTileItemsWithAreaScanningEffects();
            this.HandleTransformationEffects();
            this.HandleTileItemsWithAbsorbingEffects();
        }
        public void DestroyTileItem(TileItem tileItemToDestroy, int row, int col)
        {
            OnTileItemDestruction?.Invoke(tileItemToDestroy);
            foreach (OnDestroyTileItemEffect effect in tileItemToDestroy.Effects.OfType<OnDestroyTileItemEffect>())
            {
                effect.PerformOnDestroyAction(this);
            }
            Inventory.TileItems.Remove(tileItemToDestroy);
            SlotMachine.SetTileItemNull(row, col);
        }
        public void AddTileItemToInventory(TileItem item)
        {
            TileItem itemToAdd = _tileItemsCollection[item.Name]();

            Inventory.TileItems.Add(itemToAdd);
            OnInventoryItemAdded?.Invoke(itemToAdd);
        }
        public void AddRelicToInventory(Relic relic)
        {
            Relic relicToAdd = _relicsCollection[relic.Name]();

            Inventory.Relics.Add(relicToAdd);
            OnInventoryItemAdded?.Invoke(relicToAdd);

            if (relicToAdd.Effects.Any(e => e is GameStatRelicEffect))
                StatsTracker.SetChanged();
        }
        public bool RemoveTileItemFromInventory(TileItem tileItem)
        {
            if (!Inventory.TileItems.Contains(tileItem))
                return false;
            Inventory.TileItems.Remove(tileItem);
            OnInventoryItemRemoved?.Invoke(tileItem);
            return true;
        }
        public bool RemoveRelicFromInventory(Relic relic)
        {
            if (!Inventory.Relics.Contains(relic))
                return false;

            if (relic.Effects.Any(e => e is GameStatRelicEffect))
                StatsTracker.SetChanged();

            Inventory.Relics.Remove(relic);
            OnInventoryItemRemoved?.Invoke(relic);

            return true;
        }
        public bool RemoveTileItemFromInventoryById(string tileItemId)
        {
            TileItem item = Inventory.TileItems.FirstOrDefault(ti => ti.Id == tileItemId);
            if (item == null)
                return false;

            return RemoveTileItemFromInventory(item);
        }
        public bool RemoveRelicFromInventoryById(string relicId)
        {
            Relic relic = Inventory.Relics.FirstOrDefault(r => r.Id == relicId);
            if (relic == null)
                return false;

            return RemoveRelicFromInventory(relic);
        }
        public void ReplaceTileItem(TileItem oldTileItem, TileItem newTileItem)
        {
            int index = Inventory.TileItems.IndexOf(oldTileItem);
            Inventory.TileItems[index] = newTileItem;
        }
        public void AddCoins(int count) =>
            CoinsCount += count;
        public bool UseToken(TokenType token)
        {
            if (Inventory.Tokens.TryUseToken(token))
            {
                this.TriggerOnTokenUsedEffects(token);
                OnTokenUsed?.Invoke(token);
                return true;
            }
            return false;
        }


        public void SetCurrentCoinsCount(int value) =>
            CoinsCount = value;

        public event StageStartedDelegate OnNewStageStarted;
        public delegate void StageStartedDelegate(int newStageNumber);

        public event Action<TileItem> OnTileItemDestruction;
        public event Action<BaseInventoryItem> OnInventoryItemAdded;
        public event Action<BaseInventoryItem> OnInventoryItemRemoved;

        public event Action<TokenType> OnTokenUsed;


    }
}
