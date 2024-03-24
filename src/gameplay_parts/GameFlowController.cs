using SaYSpin.src.abstract_classes;
using SaYSpin.src.gameplay_parts.inventory_related;

namespace SaYSpin.src.gameplay_parts
{
    public class GameFlowController
    {

        private Difficulty _difficulty { get; init;}
        public SlotMachine SlotMachine { get; private set; }
        public Inventory Inventory { get; private set; }
        public short SpinsLeft { get; private set; }
        public int CurrentStage { get; private set; }
        public int CoinsCount { get; private set; }
        public int CoinsNeededToCompleteTheStage { get; private set; }
        public GameFlowController(GameStarterKit starterKit, Difficulty difficulty )
        {
            _difficulty = difficulty;

            Inventory = new(starterKit.TileItems, starterKit.Relics, starterKit.TokensCollection, starterKit.DiamondsCount);
            SlotMachine = new(Inventory.TileItems, 3, 3);

            CurrentStage = 0;
            StartNewStage();

        }
        private int CalculateStageStartingCoins()
        {
            //will change according to bonuses
            return 0;
        }
        private int CalculateCoinsNeededForStage(int stageToCalculateFor)
        {
            //will change according to bonuses
            return (int)(Math.Pow(stageToCalculateFor, 1.8) * (_difficulty.NeededCoinsMultiplier + 1) * 3.2) + 10;
        }
        public void StartNewStage()
        {

            CurrentStage += 1;
            SpinsLeft = 7;
            CoinsCount = CalculateStageStartingCoins();
            CoinsNeededToCompleteTheStage = CalculateCoinsNeededForStage(CurrentStage);
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
