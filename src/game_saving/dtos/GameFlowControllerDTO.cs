using SaYSpin.src.gameplay_parts;
using SaYSpin.src.gameplay_parts.game_flow_controller;
using SaYSpin.src.gameplay_parts.run_progress;
using SaYSpin.src.gameplay_parts.shop;
using SaYSpin.src.inventory_items.relics;
using SaYSpin.src.inventory_items.tile_items;
using static SaYSpin.src.gameplay_parts.game_flow_controller.GameFlowController;

namespace SaYSpin.src.game_saving.dtos
{
    internal class GameFlowControllerDTO
    {
        public string[] AllTileItemsCollection { get; init; }
        public string[] AllRelicsCollection { get; init; }
        public Difficulty Difficulty { get; init; }
        public SlotMachineDTO SlotMachine { get; init; }
        public InventoryDTO Inventory { get; init; }
        public short SpinsLeft { get; init; }
        public int CurrentStage { get; init; }
        public int CoinsCount { get; init; }
        public int CoinsNeededToCompleteTheStage { get; init; }
        public RunProgressController RunProgressController { get; init; }
        public StatsTracker StatsTracker { get; init; }
        public ShopController Shop { get; init; }
        public bool AreCheatsEnabled { get; init; }
        public static GameFlowControllerDTO FromGameFlowController(GameFlowController gameFlowController) => new()
        {
            AllTileItemsCollection = gameFlowController.AllTileItemsCollection.Select(ti => ti.Name).ToArray(),
            AllRelicsCollection = gameFlowController.AllRelicsCollection.Select(r => r.Name).ToArray(),
            Difficulty = gameFlowController.Difficulty,
            SlotMachine = SlotMachineDTO.FromSlotMachine(gameFlowController.SlotMachine),
            Inventory = InventoryDTO.FromInventory(gameFlowController.Inventory),
            SpinsLeft = gameFlowController.SpinsLeft,
            CurrentStage = gameFlowController.CurrentStage,
            CoinsCount = gameFlowController.CoinsCount,
            CoinsNeededToCompleteTheStage = gameFlowController.CoinsNeededToCompleteTheStage,
            RunProgressController = gameFlowController.RunProgressController,
            StatsTracker = gameFlowController.StatsTracker,
            Shop = gameFlowController.Shop,
            AreCheatsEnabled = gameFlowController.AreCheatsEnabled
        };

        public GameFlowController ToGameFlowController(
            BeforeNewStageDialogDelegate ShowBeforeStageDialog,
            Dictionary<string, Func<TileItem>> tileItemConstructors,
            Dictionary<string, Func<Relic>> relicConstructors)
        {
            tileItemConstructors = tileItemConstructors.Where(tiFunc => AllTileItemsCollection.Contains(tiFunc.Key)).ToDictionary();
            relicConstructors = relicConstructors.Where(rFunc => AllRelicsCollection.Contains(rFunc.Key)).ToDictionary();
            return new GameFlowController(
                tileItemConstructors,
                relicConstructors,
                Difficulty,
                SlotMachine.ToSlotMachine(tileItemConstructors),
                Inventory.ToInventory(tileItemConstructors, relicConstructors),
                SpinsLeft,
                CurrentStage,
                CoinsCount,
                CoinsNeededToCompleteTheStage,
                RunProgressController,
                StatsTracker,
                Shop,
                AreCheatsEnabled,
                ShowBeforeStageDialog
            );
        }
    }
}
