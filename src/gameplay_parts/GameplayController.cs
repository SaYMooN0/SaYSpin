using SaYSpin.src.abstract_classes;
using SaYSpin.src.gameplay_parts.inventory_related;

namespace SaYSpin.src.gameplay_parts
{
    public class GameplayController
    {

        private ushort _difficulty = 1;
        public SlotMachine SlotMachine { get; private set; }
        public Inventory Inventory { get; private set; }
        public int CurrentStage { get; private set; }
        public int CoinsCount { get; private set; }
        public int CoinsNeededToCompleteTheStage{  get; private set; }    
        public GameplayController(ushort difficulty = 1, List<BaseTileItem>? startingTileItems = null, List<BaseRelic>? startingRelics = null)
        {
            _difficulty = difficulty;

            OrdinaryTileItem t1 = new("i:1", "1.png", 1);
            OrdinaryTileItem t2 = new("i:2", "2.png", 2);
            OrdinaryTileItem t3 = new("i:3", "3.png", 3);

            if (startingTileItems is null)
                startingTileItems = [t1, t2, t3];
            else
                startingTileItems.AddRange([t1, t2, t3]);

            Inventory = new(startingTileItems, startingRelics);
            SlotMachine = new(Inventory.TileItems, 3, 3);

            CurrentStage = 0;
            CoinsCount = 10;
            CoinsNeededToCompleteTheStage = 100;
        }


        //public event StageStartedDelegate NewStageStarted;
        //public delegate void StageStartedDelegate(int newStageNumber);



    }
}
