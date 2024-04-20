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
        public Dictionary<string, Func<TileItem>> TileItemsCollection { get; init; }
        public List<TileItem> TileItems { get; init; }
        public HashSet<Relic> Relics { get; init; }
        public Difficulty Difficulty { get; init; }
        public SlotMachine SlotMachine { get; private set; }
        public Inventory Inventory { get; private set; }
        public short SpinsLeft { get; private set; }
        public int CurrentStage { get; private set; }
        public int CoinsCount { get; private set; }
        public int CoinsNeededToCompleteTheStage { get; private set; }
        public StatsTracker StatsTracker { get; init; }
        public bool AreCheatsEnabled { get; init; }

        public GameFlowController(StatsTracker stats, Difficulty difficulty, Dictionary<string, Func<TileItem>> accessibleTileItems, IEnumerable<Relic> accessibleRelics, bool areCheatsEnabled)
        {
            StatsTracker = stats;
            Difficulty = difficulty;
            TileItemsCollection = accessibleTileItems;
            TileItems = accessibleTileItems.Select(ti => ti.Value()).ToList();
            Relics = accessibleRelics.ToHashSet();
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
            int diamondsFromCoins = (int)(extraCoins / (CurrentStage + 4) * 1.4 * StatsTracker.AfterStageCoinsToDiamondsCoefficient);
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
            var itemToAdd = TileItemsCollection[item.Name]();
            Inventory.TileItems.Add(itemToAdd);
            OnInventoryItemAdded?.Invoke(itemToAdd);
        }
        public void AddRelicToInventory(Relic relic)
        {
            Inventory.Relics.Add(relic);
            OnInventoryItemAdded?.Invoke(relic);

            if (relic.Effects.Any(e => e is GameStatRelicEffect))
                StatsTracker.SetChanged();
        }

        public bool RemoveTileItemFromInventory(string tileItemId)
        {
            var ti = Inventory.TileItems.FirstOrDefault(ti => ti.Id == tileItemId);
            if (ti is null)
                return false;
            Inventory.TileItems.Remove(ti);
            OnInventoryItemRemoved?.Invoke(ti);
            return true;
        }
        public bool RemoveRelicFromInventory(string relicId)
        {
            var relic = Inventory.Relics.FirstOrDefault(r => r.Id == relicId);
            if (relic is null)
                return false;

            Inventory.Relics.Remove(relic);
            OnInventoryItemRemoved?.Invoke(relic);

            if (relic.Effects.Any(e => e is GameStatRelicEffect))
                StatsTracker.SetChanged();
            return true;

        }
        public void AddCoins(int count) =>
            CoinsCount += count;
        public bool UseToken(TokenType token)
        {
            if (Inventory.Tokens.TryUseToken(token))
            {
                OnTokenUsed?.Invoke(token);
                foreach (var effect in Inventory.Relics.SelectMany(r => r.Effects.OfType<AfterTokenUsedRelicEffect>()))
                {
                    effect.PerformAfterTokenUsedAction(this);
                }
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
