using SaYSpin.Components.dialogs;
using SaYSpin.src.extension_classes;
using SaYSpin.src.game_saving.dtos;
using SaYSpin.src.gameplay_parts.game_flow_controller;
using SaYSpin.src.inventory_items.relics;
using SaYSpin.src.inventory_items.tile_items;
using System.Text.Json;
using static SaYSpin.src.gameplay_parts.game_flow_controller.GameFlowController;

namespace SaYSpin.src.game_saving
{
    static internal class SavingController
    {
        private const string FILE_TO_SAVE_RUN = "savedrun";
        internal static void SaveGame(GameFlowController game)
        {
            GameFlowControllerDTO dtoToSave = DTOExtensions.GameFlowControllerToDTO(game);
            string serialized = JsonSerializer.Serialize(dtoToSave);
            File.WriteAllText(FILE_TO_SAVE_RUN, serialized);
        }
        internal static GameFlowController? LoadSavedGame(
            BeforeNewStageDialogDelegate beforeStageActionDialog,
            Dictionary<string, Func<TileItem>> tileItemsConstructors,
            Dictionary<string, Func<Relic>> relicConstructors)
        {
            if (!File.Exists(FILE_TO_SAVE_RUN)) return null;
            try
            {
                string serialized = File.ReadAllText(FILE_TO_SAVE_RUN);
                GameFlowControllerDTO? dto = JsonSerializer.Deserialize<GameFlowControllerDTO>(serialized);
                return dto is null ?
                    null :
                    DTOExtensions.GameFlowControllerFromDTO(dto, beforeStageActionDialog, tileItemsConstructors, relicConstructors);
            }
            catch
            {
                return null;
            }
        }
        internal static void DeleteSavedGame() => File.Delete(FILE_TO_SAVE_RUN);
        public static bool AnySavedGameExists()
        {
            if(! File.Exists(FILE_TO_SAVE_RUN))
                return false;
            string serialized = File.ReadAllText(FILE_TO_SAVE_RUN);
            GameFlowControllerDTO? dto = JsonSerializer.Deserialize<GameFlowControllerDTO>(serialized);
            return dto is not null;
        }
    }
}
