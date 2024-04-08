using SaYSpin.src.gameplay_parts.inventory_related;
using SaYSpin.src.secondary_classes;
using SaYSpin.src.extension_classes;
using SaYSpin.src.inventory_items.tile_items;
using SaYSpin.src.inventory_items.relics;
using SaYSpin.src.inventory_items.relics.relic_effects;
using SaYSpin.src.inventory_items.tile_items.tile_item_effects;
using SaYSpin.src.inventory_items;

namespace SaYSpin.src.gameplay_parts
{
    public class GameFlowController
    {
        public HashSet<TileItem> TileItems { get; init; }
        public HashSet<Relic> Relics { get; init; }
        public Difficulty Difficulty { get; init; }
        public SlotMachine SlotMachine { get; private set; }
        public Inventory Inventory { get; private set; }
        public short SpinsLeft { get; private set; }
        public int CurrentStage { get; private set; }
        public int CoinsCount { get; private set; }
        public int CoinsNeededToCompleteTheStage { get; private set; }
        public BasicStats BasicStats { get; init; }
        public event Action<TileItem> OnTileItemDestruction;
        public event Action<BaseInventoryItem> OnInventoryItemAdded;
        public GameFlowController(BasicStats stats, Difficulty difficulty, IEnumerable<TileItem> accessibleTileItems, IEnumerable<Relic> accessibleRelics)
        {
            BasicStats = stats;
            Difficulty = difficulty;
            TileItems = accessibleTileItems.ToHashSet();
            Relics = accessibleRelics.ToHashSet();

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
            int diamondsFromCoins = (int)(extraCoins / (CurrentStage + 4) * 1.4 * this.CoinsToDiamondsCoefficient());
            int diamondsFromSpins = (int)((CurrentStage + 4) / 4.5 * SpinsLeft);

            var rewards = this.GatherAllAfterStageRewards(CurrentStage);

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
            CurrentStage += 1;
            SpinsLeft = 7;
            CoinsCount = 0;
            CoinsNeededToCompleteTheStage = this.CalculateCoinsNeededForStage(CurrentStage);
            OnNewStageStarted?.Invoke(CurrentStage);

        }
        public void SpinSlotMachine()
        {
            TileItemsPicker itemsPicker = new(SlotMachine.TotalSlots);
            itemsPicker.PickItemsFrom(Inventory.TileItems);
            SlotMachine.UpdateItems(itemsPicker);

            IEnumerable<CoinsCalculationRelicEffect> relicEffects = Inventory.Relics
                .SelectMany(relic => relic.Effects.OfType<CoinsCalculationRelicEffect>());


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
            Inventory.TileItems.Add(item);
            OnInventoryItemAdded?.Invoke(item);
        }
        public void AddRelicToInventory(Relic relic)
        {
            Inventory.Relics.Add(relic);
            OnInventoryItemAdded?.Invoke(relic);
        }
        public void AddCoins(int count) =>
            CoinsCount += count;


        public event StageStartedDelegate OnNewStageStarted;
        public delegate void StageStartedDelegate(int newStageNumber);


    }
}
