using SaYSpin.src.enums;
using SaYSpin.src.extension_classes;
using SaYSpin.src.gameplay_parts;
using SaYSpin.src.inventory_items.relics;
using SaYSpin.src.inventory_items.tile_items;
using SaYSpin.src.extension_classes.factories;
using SaYSpin.src.in_game_logging_related;
using SaYSpin.src.static_classes;
using SaYSpin.src.gameplay_parts.inventory_related.tokens;
using SaYSpin.src.inventory_items_storages;

namespace SaYSpin.src.singletons
{
    public class AppController
    {
        public GameFlowController? Game { get; private set; }
        private TileItemsStorage TileItemsStorage { get; init; }
        public Relic[] AllRelicsCollection { get; init; }
        public Difficulty[] PossibleDifficulties { get; init; }
        public AppController()
        {
            TileItemsStorage = new();
            AllRelicsCollection = InitRelics();

            PossibleDifficulties = InitDifficulties();
        }

        private Difficulty[] InitDifficulties()
        {
            Difficulty normalDiff = Difficulty.NormalDifficulty;
            Difficulty hardDiff = Difficulty.New("Hard", 1.2, 1.2, 10, 3, 3, 1, 1.2, 1.2);
            Difficulty easyDiffi = Difficulty.New("Easy", 0.95, 1, 15, 3, 3, 2, 1, 1);
            return [easyDiffi, normalDiff, hardDiff];
        }

        private Relic[] InitRelics()
        {
            var fruitBasket = new Relic("Fruit Basket", Rarity.Common)
                .WithCoinsCalculationRelicEffect("All fruits give +1 coin", ModifierType.Plus, 1, i => i.HasTag("fruit"));
            var birdGuide = new Relic("Bird Guide", Rarity.Common)
                .WithCoinsCalculationRelicEffect("All birds give +1 coin", ModifierType.Plus, 1, i => i.HasTag("bird"));

            var treasureMap = new Relic("Treasure Map", Rarity.Rare)
                .WithAfterStageRewardRelicEffect("After every stage completion have a 50% chance to receive a chest tile item and 10% chance to receive golden chest tile item",
                    (stageNumber, game) => [
                        Randomizer.Percent(50)? game.TileItemWithId("chest")        : null ,
                        Randomizer.Percent(10)? game.TileItemWithId("golden_chest") : null]
                );



            var goldenKey = new Relic("Golden Key", Rarity.Rare)
                .WithAfterSpinRelicEffect("After each spin have a 30% chance to open a chest in a slot machine field",
                    game =>
                    {
                        for (int row = 0; row < game.SlotMachine.RowsCount; row++)
                        {
                            for (int col = 0; col < game.SlotMachine.ColumnsCount; col++)
                            {
                                var tileItem = game.SlotMachine.TileItems[row, col];
                                if (tileItem.IsChest() && Randomizer.Percent(30))
                                {
                                    game.DestroyTileItem(tileItem, row, col);
                                }
                            }
                        }
                    }
                );

            var appleTree = new Relic("Apple Tree", Rarity.Epic)
                .WithCoinsCalculationRelicEffect(
                    "Apple tile item gives +1 coins",
                    ModifierType.Plus, 1, i => i.Id == "apple")
                .WithAfterStageRewardRelicEffect(
                    "After stage have a 40% chance to receive an apple tile item",
                    (stageNumber, game) =>
                        [Randomizer.Percent(50) ? game.TileItemWithId("apple") : null])
                .WithAfterStageRewardRelicEffect(
                    "After every fifth stage receive a golden apple tile item",
                    (stageNumber, game) =>
                        [stageNumber % 5 == 0 ? game.TileItemWithId("golden_apple") : null]
                );

            var randomToken = new Relic("Random Token", Rarity.Epic)
                    .WithOnStageStartedRelicEffect(
                        "At the beginning of each stage, receive 1 random token",
                        (game) => game.Inventory.Tokens.AddToken(TokensCollection.RandomTokenType())
            );

            var diamondToken = new Relic("Diamond Token", Rarity.Rare)
                .WithAfterTokenUsedRelicEffect(
                    "When using any token, receive from 5 to 7 diamonds",
                        (game) => game.Inventory.AddDiamonds(Randomizer.Int(5, 7)))
                .WithGameStatRelicEffect("Receive 5% more diamonds for extra coins after each stage", GameStat.AfterStageCoinsToDiamondsCoefficient, ModifierType.Plus, 0.05);

            var ufo = new Relic("UFO", Rarity.Epic)
                .WithNonConstantCalculationRelicEffect(
                    "Aliens give additional 0.5 coins for each alien in the inventory",
                    (game) =>
                        {
                            int aliensCount = game.Inventory.TileItems.Where(ti => ti.IsAlien()).Count();
                            return new(ModifierType.Plus, aliensCount * 0.5);
                        },
                    ti => ti.IsAlien()
                );
            var clover = new Relic("Four Leaf Clover", Rarity.Rare)
                .WithGameStatRelicEffect("Gives +5 to luck", GameStat.Luck, ModifierType.Plus, 5);
            var milk = new Relic("Milk", Rarity.Common)
                .WithCoinsCalculationRelicEffect("All humans give 15% more coins", ModifierType.Multiply, 1.15, (ti) => ti.HasTag("human"));

            var telescope = new Relic("Telescope", Rarity.Rare)
                .WithCoinsCalculationRelicEffect("All planets give 1.25 × coins", ModifierType.Multiply, 1.15, (ti) => ti.HasTag("planet"));
            return [
                fruitBasket,
                treasureMap,
                goldenKey,
                birdGuide,
                appleTree,
                randomToken,
                diamondToken,
                ufo,
                clover,
                milk,
                telescope
                ];
        }
        public bool IsGameRunning() => Game is not null;
        public void InitializeNewGame(Difficulty difficulty, GameLoggingService logger, bool areCheatsEnabled)
        {
            logger.Clear();

            Game = new(LoadInitialStats(), difficulty, TileItemsStorage.GetAvailableItems(), AllRelicsCollection, areCheatsEnabled);

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

