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

            AllTileItemsCollection = InitTileItems();

            RelicWithCalculationEffect fruitBasket = new("fruit_basket", "Fruit Basket", "All fruits receive +1", Rarity.Common,
                new TagCalculationEffect(i => i.CoinValue + 1, i => i.HasTag("fruit")));
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

            OrdinaryTileItem appleTileItem = new("apple", "Apple", Rarity.Common, 1, ["fruit"]);
            OrdinaryTileItem orangeTileItem = new("orange", "Orange", Rarity.Common, 1, ["fruit"]);
            OrdinaryTileItem dragonFruitTileItem = new("dragon_fruit", "dragon_fruit.png", Rarity.Rare, 3, ["fruit"]);

            OrdinaryTileItem t1 = new("1", "medal 1", Rarity.Common, 1, null);
            OrdinaryTileItem t2 = new("2", "medal 2", Rarity.Common, 2, null);
            OrdinaryTileItem t3 = new("3", "medal 3", Rarity.Common, 3, null);
            OrdinaryTileItem t4 = new("4", "medal 3", Rarity.Common, 4, null);
            OrdinaryTileItem t5 = new("5", "medal 3", Rarity.Rare, 5, null);

            return [appleTileItem, orangeTileItem, dragonFruitTileItem, t1, t2, t3, t4, t5];
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
            List<GameStarterKit> kits = new();
            var commonItems = AllTileItemsCollection.Where(i => i.Rarity == Rarity.Common).OrderBy(x => Guid.NewGuid()).ToList();
            var rareItems = AllTileItemsCollection.Where(i => i.Rarity == Rarity.Rare).OrderBy(x => Guid.NewGuid()).ToArray();

            var relics = AllRelicsCollection.Where(r => r.Rarity <= Rarity.Rare).OrderBy(x => Guid.NewGuid()).ToArray();

            var totalKits = 4;
            int commonItemsPerKit = 3;

            for (int i = 0; i < totalKits; i++)
            {
                var itemsForKit = commonItems.Skip(i * commonItemsPerKit).Take(commonItemsPerKit).ToList();
                itemsForKit.Add(rareItems[i % rareItems.Length]);

                kits.Add(new GameStarterKit(itemsForKit, [relics[i % relics.Length]],
                    GameStarterKit.RandomTokensCollection(difficulty.StartingTokensCount), difficulty.StartingDiamondsCount));
            }

            return kits;
        }
    }
}

