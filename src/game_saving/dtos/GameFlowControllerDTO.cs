using SaYSpin.src.gameplay_parts;
using SaYSpin.src.gameplay_parts.run_progress;
using SaYSpin.src.gameplay_parts.shop;

namespace SaYSpin.src.game_saving.dtos
{
    internal class GameFlowControllerDTO
    {
        public GameFlowControllerDTO(string[] allTileItemsCollection,
            string[] allRelicsCollection,
            Difficulty difficulty,
            SlotMachine slotMachine,
            InventoryDTO inventory,
            short spinsLeft,
            int currentStage,
            int coinsCount,
            int coinsNeededToCompleteTheStage,
            RunProgressController runProgressController,
            StatsTracker statsTracker,
            ShopController shop,
            bool areCheatsEnabled)
        {
            AllTileItemsCollection = allTileItemsCollection;
            AllRelicsCollection = allRelicsCollection;
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
        }

        public string[] AllTileItemsCollection { get; init; }
        public string[] AllRelicsCollection { get; init; }
        public Difficulty Difficulty { get; init; }
        public SlotMachine SlotMachine { get; init; }
        public InventoryDTO Inventory { get; init; }
        public short SpinsLeft { get; init; }
        public int CurrentStage { get; init; }
        public int CoinsCount { get; init; }
        public int CoinsNeededToCompleteTheStage { get; init; }
        public RunProgressController RunProgressController { get; init; }
        public StatsTracker StatsTracker { get; init; }
        public ShopController Shop { get; init; }
        public bool AreCheatsEnabled { get; init; }
    }
}
