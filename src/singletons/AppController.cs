using SaYSpin.src.gameplay_parts;
using SaYSpin.src.game_logging;
using SaYSpin.src.inventory_items_storages;
using SaYSpin.src.gameplay_parts.game_flow_controller;
using SaYSpin.src.gameplay_parts.shop;

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
        public void InitializeNewGame(Difficulty difficulty, GameLoggingService logger, BeforeStageActionDialogService beforeStageActionDialogService, bool areCheatsEnabled)
        {
            logger.Clear();

            Game = new(
                LoadInitialStats(difficulty),
                difficulty,
                TileItemsStorage.GetAvailableItems(),
                RelicsStorage.GetAvailableItems(),
                LoadSpecialMerchants(),
                beforeStageActionDialogService.ShowDialog,
                areCheatsEnabled
               );

            Game.OnNewStageStarted += (newStage) => logger.Log(GameLogModel.New($"Stage #{newStage} has been started", GameLogType.Info));
            Game.OnInventoryItemAdded += logger.LogItemAdded;

            Game.OnTileItemDestruction += logger.LogItemDestroyed;
            Game.OnTokenUsed += logger.LogTokenUsed;

        }
        public void FinishGame()
        {
            //TODO :
            //invoke in finish event
            //count exp and rubies
            //only then null

            Game = null;
        }
        private ISpecialMerchant[] LoadSpecialMerchants() => [];
        private StatsTracker LoadInitialStats(Difficulty difficulty)
        {
            //TODO : StatsTracker from file
            return new StatsTracker(
                initLuck: 1,
                initNewStageTileItemsForChoiceCount: 4,
                initNewStageRelicsForChoiceCount: 4,
                initStageSpinsCount: 7,
                initAfterStageCoinsToDiamondsCoefficient: 1,
                initShopPriceCoefficient: 1,
                initTileItemsInShopCount: 4,
                initRelicsInShopCount: 2,
                initCoinsNeededToCompleteStage: difficulty.NeededCoinsMultiplier
                );
        }


    }
}

