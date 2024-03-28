using SaYSpin.src.abstract_classes;
using SaYSpin.src.coins_calculation_related.specific_calculation_effects;
using SaYSpin.src.extension_classes;
using SaYSpin.src.gameplay_parts;
using SaYSpin.src.gameplay_parts.inventory_related.relics;
using SaYSpin.src.gameplay_parts.inventory_related.tile_items;

namespace SaYSpin.src.singletons
{
    public class AllGameController
    {
        public GameFlowController? Game { get; private set; }
        public BaseTileItem[] AllTileItemsCollection { get; init; }
        public BaseRelic[] AllRelicsCollection { get; init; }
        public Difficulty[] PossibleDifficulties { get; init; }
        private BasicStats _basicStats { get; set; }
        public AllGameController()
        {
            _basicStats = LoadBasicStats();

            AllTileItemsCollection = InitTileItems();

            RelicWithCalculationEffect fruitBasket = new("Fruit Basket", "All fruits gives +1 coin", Rarity.Common,
                new TagCalculationEffect(i => i.InitialCoinValue + 1, i => i.HasTag("fruit")));
            AllRelicsCollection = [fruitBasket];

            Difficulty normalDifficulty = new("normal", [], [], "normal.png", 1, 1, 10, 3, 1, 1);
            Difficulty hardDifficulty = new("hard",
                ["Receive 1,2 x more rubies after run", "Receive 1,2 x more exp after run"],
                ["More coins are needed to complete each stage", "Increased prices in the shop"],
                "normal.png", 1.2, 1.2, 10, 3, 1.2, 1.2);
            PossibleDifficulties = [normalDifficulty, hardDifficulty];
        }

        private BaseTileItem[] InitTileItems()
        {

            OrdinaryTileItem t1 = new("medal 1", Rarity.Common, 1, null);
            OrdinaryTileItem t2 = new("medal 2", Rarity.Common, 2, null);
            OrdinaryTileItem t3 = new("medal 3", Rarity.Common, 3, null);

            return [
                new OrdinaryTileItem ("Apple", Rarity.Common, 3, ["fruit"]),
                new OrdinaryTileItem ("Banana", Rarity.Common, 3, ["fruit"]),
                new OrdinaryTileItem ("Dragon Fruit", Rarity.Rare, 7, ["fruit"]),
                new OrdinaryTileItem ("Golden Apple", Rarity.Epic, 17, ["fruit","gold" ]),
                new OrdinaryTileItem ("Orange", Rarity.Common, 3, ["fruit"]),
                new OrdinaryTileItem ("Pineapple", Rarity.Rare, 5, ["fruit"]),

                new OrdinaryTileItem ("Candy", Rarity.Common, 3, ["sweet"]),
                new OrdinaryTileItem ("Chocolate Bar", Rarity.Rare, 5, ["sweet"]),
                new OrdinaryTileItem ("Lollipop", Rarity.Common, 3, ["sweet"]),
                new OrdinaryTileItem ("Sweet Tooth", Rarity.Epic, 1, ["sweet"]),//AbsorberTileItem
                
                new OrdinaryTileItem ("Pirate", Rarity.Legendary, 7, ["person"]), //AbsorberTileItem
                new OrdinaryTileItem ("Chest", Rarity.Rare, 1, [ "chest"]),
                new OrdinaryTileItem ("Golden Chest", Rarity.Epic, 3, ["chest", "gold"]),
                new OrdinaryTileItem ("Gold Bar", Rarity.Rare, 20, ["gold"]),
                t1, t2, t3];
        }
        public bool IsGameRunning() => Game is not null;
        public void InitializeNewGame(Difficulty difficulty)
        {
            Game = new(_basicStats, difficulty, AllTileItemsCollection, AllRelicsCollection);
        }
        public void FinishGame()
        {
            Logger.Log("game ended");
            Game = null;
        }
        private BasicStats LoadBasicStats()
        {
            //will be load from file
            return BasicStats.Default();
        }


    }
}

