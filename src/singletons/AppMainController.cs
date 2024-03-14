using SaYSpin.src.gameplay_parts;

namespace SaYSpin.src.singletons
{
    public class AppMainController
    {
        public GameplayController? Game { get; private set; }
        public bool IsGameRunning() => Game is not null;
        public void StartNewGame(ushort difficulty = 1)
        {
            Game = new(difficulty);
        }
        public void GameEnded()
        {
            Logger.Log("game ended");
            Game = null;
        }
    }
}
