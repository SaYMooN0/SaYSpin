using SaYSpin.src.abstract_classes;
using SaYSpin.src.gameplay_parts.inventory_related;
using SaYSpin.src.secondary_classes;
using SaYSpin.src.extension_classes;

namespace SaYSpin.src.gameplay_parts
{
    public class GameFlowController
    {
        public HashSet<BaseTileItem> TileItems { get; init; }
        public HashSet<BaseRelic> Relics { get; init; }
        public Difficulty Difficulty { get; init; }
        public SlotMachine SlotMachine { get; private set; }
        public Inventory Inventory { get; private set; }
        public short SpinsLeft { get; private set; }
        public int CurrentStage { get; private set; }
        public int CoinsCount { get; private set; }
        public int CoinsNeededToCompleteTheStage { get; private set; }
        public BasicStats BasicStats { get; init; }
        public GameFlowController(BasicStats stats, Difficulty difficulty, IEnumerable<BaseTileItem> accessibleTileItems, IEnumerable<BaseRelic> accessibleRelics)
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


            return new(
                CurrentStage,
                CoinsNeededToCompleteTheStage,
                CoinsCount,
                extraCoins,
                SpinsLeft,
                diamondsFromCoins,
                diamondsFromSpins,
                diamondsFromCoins + diamondsFromSpins);
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

            int income = SlotMachine.CalculateCoinValue();
            CoinsCount += income;
            SpinsLeft -= 1;
        }
        public bool CoinsEnoughToCompleteTheStage() =>
            CoinsCount >= CoinsNeededToCompleteTheStage;

        public event StageStartedDelegate OnNewStageStarted;
        public delegate void StageStartedDelegate(int newStageNumber);


    }
}
