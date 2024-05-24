using SaYSpin.src.game_saving.dtos;
using SaYSpin.src.inventory_items.relics;
using SaYSpin.src.inventory_items.tile_items;
using SaYSpin.src.gameplay_parts.game_flow_controller;
using static SaYSpin.src.gameplay_parts.game_flow_controller.GameFlowController;
namespace SaYSpin.src.extension_classes
{
    internal static class DTOExtensions
    {
        public static GameFlowControllerDTO GameFlowControllerToDTO(this GameFlowController gameFlowController) =>
            new GameFlowControllerDTO(
                gameFlowController.AllTileItemsCollection.Select(ti => ti.Name).ToArray(),
                gameFlowController.AllRelicsCollection.Select(r => r.Name).ToArray(),
                gameFlowController.Difficulty,
                SlotMachineDTO.FromSlotMachine(gameFlowController.SlotMachine),
                InventoryDTO.FromInventory(gameFlowController.Inventory),
                gameFlowController.SpinsLeft,
                gameFlowController.CurrentStage,
                gameFlowController.CoinsCount,
                gameFlowController.CoinsNeededToCompleteTheStage,
                gameFlowController.RunProgressController,
                gameFlowController.StatsTracker,
                gameFlowController.Shop,
                gameFlowController.AreCheatsEnabled
                );


        public static GameFlowController GameFlowControllerFromDTO(
            this GameFlowControllerDTO dto,
            BeforeNewStageDialogDelegate ShowBeforeStageDialog,
            Dictionary<string, Func<TileItem>> tileItemConstructors,
            Dictionary<string, Func<Relic>> relicConstructors)
        {
            tileItemConstructors = tileItemConstructors.Where(tiFunc => dto.AllTileItemsCollection.Contains(tiFunc.Key)).ToDictionary();
            relicConstructors = relicConstructors.Where(rFunc => dto.AllRelicsCollection.Contains(rFunc.Key)).ToDictionary();

            GameFlowController gameFlowController = new(
                tileItemConstructors,
                relicConstructors,
                dto.Difficulty,
                dto.SlotMachine.ToSlotMachine(tileItemConstructors),
                dto.Inventory.ToInventory(tileItemConstructors, relicConstructors),
                dto.SpinsLeft,
                dto.CurrentStage,
                dto.CoinsCount,
                dto.CoinsNeededToCompleteTheStage,
                dto.RunProgressController,
                dto.StatsTracker,
                dto.Shop,
                dto.AreCheatsEnabled,
                ShowBeforeStageDialog
                );
            return gameFlowController;
        }
    }
}
