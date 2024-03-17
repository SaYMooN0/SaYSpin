using SaYSpin.src.abstract_classes;
using SaYSpin.src.gameplay_parts.inventory_related;

namespace SaYSpin.src.gameplay_parts
{
    public class GameplayController
    {

        private ushort _difficulty = 1;
        public SlotMachine SlotMachine { get; private set; }
        public Inventory Inventory { get; private set; }
        public short SpinsLeft { get; private set; }
        public int CurrentStage { get; private set; }
        public int CoinsCount { get; private set; }
        public int CoinsNeededToCompleteTheStage { get; private set; }
        public GameplayController(ushort difficulty = 1, List<BaseTileItem>? startingTileItems = null, List<BaseRelic>? startingRelics = null)
        {
            _difficulty = difficulty;

            OrdinaryTileItem t1 = new("i:1", "1.png",Rarity.Common, 1);
            OrdinaryTileItem t2 = new("i:2", "2.png", Rarity.Common, 2);
            OrdinaryTileItem t3 = new("i:3", "3.png", Rarity.Common, 3);

            if (startingTileItems is null)
                startingTileItems = [t1, t2, t3];
            else
                startingTileItems.AddRange([t1, t2, t3]);

            Inventory = new(startingTileItems, startingRelics);
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
            return (int)(Math.Pow(stageToCalculateFor, 1.8) * (_difficulty + 1) * 3.2) + 10;
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
