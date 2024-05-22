using SaYSpin.src.gameplay_parts.game_flow_controller;
using System.Text.Json;

namespace SaYSpin.src.game_saving
{
    static internal class SavingController
    {
        private const string FILE_TO_SAVE_RUN = "savedrun";
        internal static void SaveGame(GameFlowController game)
        {
            string serialized = JsonSerializer.Serialize(game);
            File.WriteAllText(FILE_TO_SAVE_RUN, serialized);
        }
        internal static GameFlowController? LoadSavedGame()
        {
            if (!File.Exists(FILE_TO_SAVE_RUN)) return null;
            try
            {
                string serialized = File.ReadAllText(FILE_TO_SAVE_RUN);
                return JsonSerializer.Deserialize<GameFlowController>(serialized);
            }
            catch
            {
                return null;
            }
        }
        internal static void DeleteSavedGame() => File.Delete(FILE_TO_SAVE_RUN);
        public static bool AnySavedGameExists() => LoadSavedGame() is GameFlowController;
    }
}
