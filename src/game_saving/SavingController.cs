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
        public static readonly string SavesDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "saves");
        private static readonly string FileToSaveRun = Path.Combine(SavesDirectory, "savedrun");
        static SavingController()
        {
            if (!Directory.Exists(SavesDirectory))
            {
                Directory.CreateDirectory(SavesDirectory);
            }
        }
        internal static void SaveGame(GameFlowController game)
        {
            GameFlowControllerDTO dtoToSave = GameFlowControllerDTO.FromGameFlowController(game);
            string serialized = JsonSerializer.Serialize(dtoToSave);
            File.WriteAllText(FileToSaveRun, serialized);
        }
        internal static GameFlowController? LoadSavedGame(
            BeforeNewStageDialogDelegate beforeStageActionDialog,
            Dictionary<string, Func<TileItem>> tileItemsConstructors,
            Dictionary<string, Func<Relic>> relicConstructors)
        {
            if (!File.Exists(FileToSaveRun)) return null;
            try
            {
                string serialized = File.ReadAllText(FileToSaveRun);
                GameFlowControllerDTO? dto = JsonSerializer.Deserialize<GameFlowControllerDTO>(serialized);
                return dto is null ?
                    null : dto.ToGameFlowController(beforeStageActionDialog, tileItemsConstructors, relicConstructors);
            }
            catch
            {
                return null;
            }
        }
        internal static void DeleteSavedGame() => File.Delete(FileToSaveRun);
        public static bool AnySavedGameExists()
        {
            if (!File.Exists(FileToSaveRun))
                return false;
            string serialized = File.ReadAllText(FileToSaveRun);
            try
            {
                GameFlowControllerDTO? dto = JsonSerializer.Deserialize<GameFlowControllerDTO>(serialized);
                return dto is not null;
            }
            catch
            {
                return false;
            }

        }
    }
}
