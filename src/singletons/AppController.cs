using SaYSpin.src.gameplay_parts;
using SaYSpin.src.in_game_logging_related;
using SaYSpin.src.inventory_items_storages;

namespace SaYSpin.src.singletons
{
    public class AppController
    {
        public GameFlowController? Game { get; private set; }
        private TileItemsStorage TileItemsStorage { get; init; }
        private RelicsStorage RelicsStorage { get; init; }
        public Difficulty[] PossibleDifficulties { get; init; }
        public AppController()
        {
            TileItemsStorage = new();
            RelicsStorage = new();

            PossibleDifficulties = InitDifficulties();
        }

        private Difficulty[] InitDifficulties()
        {
            Difficulty normalDiff = Difficulty.NormalDifficulty;
            Difficulty hardDiff = Difficulty.New("Hard", 1.2, 1.2, 10, 3, 3, 1, 1.2, 1.2);
            Difficulty easyDiffi = Difficulty.New("Easy", 0.95, 1, 15, 3, 3, 2, 1, 1);
            return [easyDiffi, normalDiff, hardDiff];
        }


        public bool IsGameRunning() => Game is not null;
        public void InitializeNewGame(Difficulty difficulty, GameLoggingService logger, bool areCheatsEnabled)
        {
            logger.Clear();

            Game = new(LoadInitialStats(), difficulty, TileItemsStorage.GetAvailableItems(), RelicsStorage.GetAvailableItems(), areCheatsEnabled);

            Game.OnNewStageStarted += (newStage) => logger.Log(GameLogModel.New($"Stage #{newStage} has been started", GameLogType.Info));
            Game.OnInventoryItemAdded += logger.LogItemAdded;

            Game.OnTileItemDestruction += logger.LogItemDestroyed;
            Game.OnTokenUsed += logger.LogTokenUsed;
        }
        public void FinishGame()
        {
            //invoke event
            //count exp and rubies
            //only then null
            Game = null;
        }
        private StatsTracker LoadInitialStats()
        {
            //will be load from file
            return new(1, 4, 4, 7, 1, 1);
        }


    }
}

