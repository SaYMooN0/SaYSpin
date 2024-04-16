using SaYSpin.src.enums;
using SaYSpin.src.extension_classes;
using SaYSpin.src.gameplay_parts;
using SaYSpin.src.inventory_items.relics;
using SaYSpin.src.inventory_items.tile_items;
using SaYSpin.src.extension_classes.factories;
using SaYSpin.src.in_game_logging_related;
using SaYSpin.src.static_classes;
using SaYSpin.src.gameplay_parts.inventory_related.tokens;

namespace SaYSpin.src.singletons
{
    public class AllGameController
    {
        public GameFlowController? Game { get; private set; }
        public TileItem[] AllTileItemsCollection { get; init; }
        public Relic[] AllRelicsCollection { get; init; }
        public Difficulty[] PossibleDifficulties { get; init; }
        public AllGameController()
        {

            AllTileItemsCollection = InitTileItems();

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
                .WithCoinsCalculationRelicEffect("All planets give 1.25 times more coins", ModifierType.Multiply, 1.15, (ti) => ti.HasTag("planet"));
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
        private TileItem[] InitTileItems()
        {

            return [
                TileItem.Ordinary("Apple", Rarity.Common, 3, ["fruit"]),
                TileItem.Ordinary("Banana", Rarity.Common, 3, ["fruit"]),
                TileItem.Ordinary("Dragon Fruit", Rarity.Rare, 7, ["fruit"]),
                TileItem.Ordinary("Golden Apple", Rarity.Epic, 17, ["fruit", "gold"]),
                TileItem.Ordinary("Orange", Rarity.Common, 3, ["fruit"]),
                TileItem.Ordinary("Pineapple", Rarity.Rare, 5, ["fruit"]),

                TileItem.Ordinary("Candy", Rarity.Common, 3, ["sweet"]),
                TileItem.Ordinary("Chocolate Bar", Rarity.Rare, 5, ["sweet"]),
                TileItem.Ordinary("Lollipop", Rarity.Common, 3, ["sweet"]),
                TileItem.Ordinary("Sweet Tooth", Rarity.Epic, 1, ["human"]), //AbsorberTileItem
        
                TileItem.Ordinary("Pirate", Rarity.Legendary, 7, ["human"]), //AbsorberTileItem
                TileItem.Ordinary("Parrot", Rarity.Rare, 5 ,["bird"])
                    .WithTileItemsEnhancingTileItemEffect("Adjacent pirates give 2 times more coins", EffectApplicationArea.Adjacent, ModifierType.Multiply, 2,(ti)=>ti.IdIs("pirate")),

                TileItem.Ordinary("Chest", Rarity.Rare, 1, ["chest"])
                    .WithOnDestroyTileItemEffect("After opening gives from 20 to 50 coins and from 3 to 7 diamonds",
                        (game)=>{
                            game.AddCoins(Randomizer.Int(20,50));
                            game.Inventory.AddDiamonds(Randomizer.Int(3,7));
                        }),
                TileItem.Ordinary("Golden Chest", Rarity.Epic, 3, ["chest", "gold"])
                    .WithOnDestroyTileItemEffect("After opening gives from 50 to 150 coins and from 10 to 20 diamonds",
                        (game)=>{
                            game.AddCoins(Randomizer.Int(50,150));
                            game.Inventory.AddDiamonds(Randomizer.Int(10,20));
                        }),
                TileItem.Ordinary("Gold Bar", Rarity.Rare, 20, ["gold"]),

                TileItem.Ordinary("Pigeon", Rarity.Common, 1 ,["bird"])
                    .WithTileItemsEnhancingTileItemEffect("Adjacent birds give +2 coins ", EffectApplicationArea.Adjacent, ModifierType.Plus, 2,(ti)=> ti.HasTag("bird") ),

                 TileItem.Ordinary("Owl", Rarity.Rare, 3 ,["bird"])
                    .WithTileItemsEnhancingTileItemEffect("Adjacent wizards give extra 1.4 times more coins ", EffectApplicationArea.Adjacent, ModifierType.Multiply, 1.4,(ti)=> ti.IdIs("wizard")),


                TileItem.Ordinary("Magic Ball", Rarity.Epic, 5 ,["magical"])
                    .WithTileItemsEnhancingTileItemEffect("All adjacent items give 1.5 times more coins", EffectApplicationArea.Adjacent, ModifierType.Multiply, 1.5,(ti)=>true)
                    .WithTileItemsEnhancingTileItemEffect("Adjacent wizards give extra 2 times more coins", EffectApplicationArea.Adjacent, ModifierType.Multiply, 2,(ti)=> ti.IdIs("wizard")),

                TileItem.Ordinary("Wizard", Rarity.Rare, 15 ,["human"]),

                TileItem.Ordinary("Rabbit", Rarity.Rare, 3, ["rabbit"])
                    .WithAbsorbingTileItemEffect("Eats adjacent carrots and adds 15 coins for each", (tileItem)=>tileItem.IdIs("carrot"), (game)=>game.AddCoins(15))
                    .WithAbsorbingTileItemEffect("Eats adjacent golden carrots and adds 150 coins for each", (tileItem)=>tileItem.IdIs("golden_carrot"), (game)=>game.AddCoins(150)),

                TileItem.Ordinary("Carrot", Rarity.Common, 5, []),
                TileItem.Ordinary("Golden Carrot", Rarity.Epic, 15, []),


                TileItem.Ordinary("Capybara", Rarity.Mythic, 10 ,["animal"])
                    .WithTileItemsEnhancingTileItemEffect("Adjacent humans, animals and birds give 2 times more coins ", EffectApplicationArea.Adjacent, ModifierType.Multiply, 2,(ti)=> ti.HasOneOfTags(["bird", "animal", "human"]) ),

                TileItem.Ordinary("Green Alien", Rarity.Rare, 3 ,["alien"])
                    .WithTileItemsEnhancingTileItemEffect("All aliens give +1 coin", EffectApplicationArea.AllTiles, ModifierType.Plus, 1,(ti)=> ti.IsAlien()),
                TileItem.Ordinary("Purple Alien", Rarity.Epic, 5 ,["alien"])
                    .WithTileItemsEnhancingTileItemEffect("All aliens in a 5 by 5 square from give 1.4 times more coins", EffectApplicationArea.Square5, ModifierType.Multiply, 1.4,(ti)=> ti.IsAlien()),
                TileItem.Ordinary("Orange Alien", Rarity.Rare, 1 ,["alien"])
                    .WithTileItemsEnhancingTileItemEffect("All aliens in the same horizontal line give +3 coins", EffectApplicationArea.HorizontalLine, ModifierType.Plus, 3,(ti)=> ti.IsAlien()),
                TileItem.Ordinary("Cyan Alien", Rarity.Legendary, 2 ,["alien"])
                    .WithTileItemsEnhancingTileItemEffect("All aliens in the same vertical line give +3 coins", EffectApplicationArea.VerticalLine, ModifierType.Plus, 3,(ti)=> ti.IsAlien())
                    .WithTileItemsEnhancingTileItemEffect("All aliens in the corner tiles give 2 times more coins", EffectApplicationArea.CornerTiles, ModifierType.Multiply, 2,(ti)=> ti.IsAlien()),
                TileItem.Ordinary("Mars", Rarity.Epic, 5, ["planet"])
                    .WithTileItemsEnhancingTileItemEffect("Adjacent green aliens give 30% more coins", EffectApplicationArea.Adjacent,ModifierType.Multiply, 1.30 , (ti)=> ti.IdIs("green_alien")),
                TileItem.Ordinary("Saturn", Rarity.Epic, 5, ["planet"])
                    .WithTileItemsEnhancingTileItemEffect("Purple aliens in the same vertical line give 1.7 times more coins", EffectApplicationArea.VerticalLine,ModifierType.Multiply,  1.7, (ti)=> ti.IdIs("purple_alien"))
                    .WithTileItemsEnhancingTileItemEffect("Purple aliens in the same horizontal line give 1.7 times more coins", EffectApplicationArea.HorizontalLine,ModifierType.Multiply, 1.7, (ti)=> ti.IdIs("purple_alien")),
                TileItem.Ordinary("Venus", Rarity.Epic, 10, ["planet"])
                    .WithTileItemsEnhancingTileItemEffect("Orange aliens in a 5 by 5 area give 5 more coins", EffectApplicationArea.Square5,ModifierType.Plus, 5 , (ti)=> ti.IdIs("orange_alien")),
                TileItem.Ordinary("Neptune", Rarity.Epic, 2, ["planet"])
                    .WithTileItemsEnhancingTileItemEffect("Cyan aliens in the same horizontal line give 3 times more coins", EffectApplicationArea.HorizontalLine,ModifierType.Multiply,  3, (ti)=> ti.IdIs("cyan_alien"))
                    .WithTileItemsEnhancingTileItemEffect("Cyan aliens in the corner tiles give 5 more coins", EffectApplicationArea.CornerTiles,ModifierType.Plus, 5, (ti)=> ti.IdIs("cyan_alien")), 
                TileItem.Ordinary("Artificial Satellite", Rarity.Legendary, 10 , [])
                    .WithTileItemsEnhancingTileItemEffect("Adjacent planets give 2.5 times more coins", EffectApplicationArea.Adjacent,ModifierType.Multiply,  2.5, (ti)=> ti.IsPlanet())
                    ];



        }
        public bool IsGameRunning() => Game is not null;
        public void InitializeNewGame(Difficulty difficulty, GameLoggingService logger, bool areCheatsEnabled)
        {
            logger.Clear();

            Game = new(LoadInitialStats(), difficulty, AllTileItemsCollection, AllRelicsCollection, areCheatsEnabled);

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

