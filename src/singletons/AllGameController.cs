using SaYSpin.src.enums;
using SaYSpin.src.extension_classes;
using SaYSpin.src.gameplay_parts;
using SaYSpin.src.inventory_items.relics;
using SaYSpin.src.inventory_items.tile_items;
using SaYSpin.src.extension_classes.factories;
using SaYSpin.src.in_game_logging_related;
using SaYSpin.src.static_classes;

namespace SaYSpin.src.singletons
{
    public class AllGameController
    {
        public GameFlowController? Game { get; private set; }
        public TileItem[] AllTileItemsCollection { get; init; }
        public Relic[] AllRelicsCollection { get; init; }
        public Difficulty[] PossibleDifficulties { get; init; }
        private BasicStats _basicStats { get; set; }
        public AllGameController()
        {
            _basicStats = LoadBasicStats();

            AllTileItemsCollection = InitTileItems();

            AllRelicsCollection = InitRelics();

            Difficulty normalDifficulty = new("normal", [], [], "normal.png", 1, 1, 10, 3, 1, 1);
            Difficulty hardDifficulty = new("hard",
                ["Receive 1,2 x more rubies after run", "Receive 1,2 x more exp after run"],
                ["More coins are needed to complete each stage", "Increased prices in the shop"],
                "normal.png", 1.2, 1.2, 10, 3, 1.2, 1.2);
            PossibleDifficulties = [normalDifficulty, hardDifficulty];
        }

        private Relic[] InitRelics()
        {
            var fruitBasket = new Relic("Fruit Basket", Rarity.Common)
                .WithCoinsCalculationRelicEffect("All fruits gives +1 coin", ModifierType.Plus, 1, i => i.HasTag("fruit"));

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
            return [
                fruitBasket,
                treasureMap,
                goldenKey
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
                TileItem.Ordinary("Sweet Tooth", Rarity.Epic, 1, ["person"]), //AbsorberTileItem
        
                TileItem.Ordinary("Pirate", Rarity.Legendary, 7, ["person"]), //AbsorberTileItem
                TileItem.Ordinary("Parrot", Rarity.Rare, 5 ,["bird"])
                    .WithTileItemsEnhancingTileItemEffect("Adjacent pirates give 2 times more coins", EffectApplicationArea.Adjacent, ModifierType.Multiply, 2,(ti)=>ti.Id=="pirate"),

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

                TileItem.Ordinary("Magic Ball", Rarity.Epic, 5 ,["magical"])
                    .WithTileItemsEnhancingTileItemEffect("All adjacent items give 1.5 times more coins", EffectApplicationArea.Adjacent, ModifierType.Multiply, 1.5,(ti)=>true)
                    .WithTileItemsEnhancingTileItemEffect("Adjacent wizards give extra 2 times more coins ", EffectApplicationArea.Adjacent, ModifierType.Multiply, 2,(ti)=> ti.Id=="wizard"),

                TileItem.Ordinary("Owl", Rarity.Rare, 3 ,["bird"])
                    .WithTileItemsEnhancingTileItemEffect("Adjacent wizards give extra 1.4 times more coins ", EffectApplicationArea.Adjacent, ModifierType.Multiply, 1.4,(ti)=> ti.Id=="wizard"),

                TileItem.Ordinary("Wizard", Rarity.Rare, 15 ,["person"]),

                TileItem.Ordinary("Rabbit", Rarity.Rare, 3, ["rabbit"])
                    .WithAbsorbingTileItemEffect("Eats adjacent carrots and adds 15 coins for each", (tileItem)=>tileItem.Id=="carrot", (game)=>game.AddCoins(15))
                    .WithAbsorbingTileItemEffect("Eats adjacent golden carrots and adds 150 coins for each", (tileItem)=>tileItem.Id=="golden_carrot", (game)=>game.AddCoins(150)),

                TileItem.Ordinary("Carrot", Rarity.Common, 5, []),
                TileItem.Ordinary("Golden Carrot", Rarity.Epic, 15, []),
                ];
        }
        public bool IsGameRunning() => Game is not null;
        public void InitializeNewGame(Difficulty difficulty, GameLoggingService logger)
        {
            logger.Clear();

            Game = new(_basicStats, difficulty, AllTileItemsCollection, AllRelicsCollection);

            Game.OnNewStageStarted += (newStage) => logger.Log(GameLogModel.New($"Stage #{newStage} has been started", GameLogType.Info));
            Game.OnInventoryItemAdded += logger.LogItemAdded;
            Game.OnTileItemDestruction += logger.LogItemDestroyed;
        }
        public void FinishGame()
        {
            Game = null;
        }
        private BasicStats LoadBasicStats()
        {
            //will be load from file
            return BasicStats.Default();
        }


    }
}

