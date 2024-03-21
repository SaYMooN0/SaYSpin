using SaYSpin.src.abstract_classes;
using SaYSpin.src.coins_calculation_related.specific_calculation_effects;
using SaYSpin.src.extension_classes;
using SaYSpin.src.gameplay_parts;
using SaYSpin.src.gameplay_parts.inventory_related;
using SaYSpin.src.gameplay_parts.inventory_related.relics;
using SaYSpin.src.gameplay_parts.inventory_related.tile_items;

namespace SaYSpin.src.singletons
{
    public class AppMainController
    {
        public AppMainController()
        {

            OrdinaryTileItem appleTileItem = new("apple", "Apple", "apple.png", Rarity.Common, 1, ["fruit"]);
            OrdinaryTileItem orangeTileItem = new("orange", "Orange", "orange.png", Rarity.Common, 1, ["fruit"]);
            OrdinaryTileItem dragonFruitTileItem = new("dragon_fruit", "Dragon Fruit", "dragon_fruit.png", Rarity.Rare, 3, ["fruit"]);

            OrdinaryTileItem t1 = new("1", "medal 1", "1.png", Rarity.Common, 1, null);
            OrdinaryTileItem t2 = new("2", "medal 2", "2.png", Rarity.Common, 2, null);
            OrdinaryTileItem t3 = new("3", "medal 3", "3.png", Rarity.Common, 3, null);
            OrdinaryTileItem t4 = new("4", "medal 3", "3.png", Rarity.Common, 4, null);
            OrdinaryTileItem t5 = new("5", "medal 3", "3.png", Rarity.Rare, 5, null);

            AllTileItemsCollection = [appleTileItem, orangeTileItem, t1, t2, t3, t4, t5];

            RelicWithCalculationEffect fruitBasket = new("fruit_basket", "Fruit Basket", "All fruits receive +1", "fruit_basket.png", Rarity.Common,
                new TagCalculationEffect(i => i.CoinValue + 1, i => i.HasTag("fruit")));
            AllRelicsCollection = [fruitBasket];

            Difficulty normalDifficulty = new("normal", [], [], "normal.png", 1, 1, 10, 3, 1, 1);
            Difficulty hardDifficulty = new("hard",
                ["Receive 1,2 x more rubies after run", "Receive 1,2 x more exp after run"],
                ["More coins are needed to complete each stage", "Increased prices in the shop"],
                "normal.png", 1.2, 1.2, 10, 3, 1.2, 1.2);
            PossibleDifficulties = [normalDifficulty, hardDifficulty];
        }

        public GameplayController? Game { get; private set; }
        public BaseTileItem[] AllTileItemsCollection { get; init; }
        public BaseRelic[] AllRelicsCollection { get; init; }
        public Difficulty[] PossibleDifficulties { get; init; }

        public bool IsGameRunning() => Game is not null;
        public void StartNewGame(List<BaseTileItem> startingItems, List<BaseRelic> startingRelics, Difficulty difficulty)
        {
            Game = new(startingItems, startingRelics, difficulty);
        }

        public void GameEnded()
        {
            Logger.Log("game ended");
            Game = null;
        }
        public List<GameStarterKit> GenerateStarterKits(Difficulty difficulty)
        {
            //temporary implementation
            var commonItems = AllTileItemsCollection.Where(i => i.Rarity == Rarity.Common).ToList();
            var rareItems = AllTileItemsCollection.Where(i => i.Rarity == Rarity.Common).ToList();

            List<BaseTileItem> gItems = commonItems[1..4];
            gItems.Add(rareItems[0]);

            GameStarterKit g = new(gItems, [AllRelicsCollection[0]], GameStarterKit.RandomTokensCollection(2),0,10 );
            return [g];
        }
    }
}
