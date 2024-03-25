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
        public AllGameController()
        {

            AllTileItemsCollection = InitTileItems();

            RelicWithCalculationEffect fruitBasket = new("Fruit Basket", "All fruits gives +1 coin", Rarity.Common,
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

            OrdinaryTileItem t1 = new("medal 1", Rarity.Common, 1, null);
            OrdinaryTileItem t2 = new("medal 2", Rarity.Common, 2, null);
            OrdinaryTileItem t3 = new("medal 3", Rarity.Common, 3, null);

            return [
                new OrdinaryTileItem ("Apple", Rarity.Common, 3, ["fruit"]),
                new OrdinaryTileItem ("Banana", Rarity.Common, 3, ["fruit"]),
                new OrdinaryTileItem ("Dragon Fruit", Rarity.Rare, 7, ["fruit"]),
                new OrdinaryTileItem ("Golden Apple", Rarity.Epic, 17, ["fruit"]),
                new OrdinaryTileItem ("Orange", Rarity.Common, 3, ["fruit"]),
                new OrdinaryTileItem ("Pineapple", Rarity.Rare, 5, ["fruit"]),

                new OrdinaryTileItem ("Candy", Rarity.Common, 3, ["sweet"]),
                new OrdinaryTileItem ("Chocolate Bar", Rarity.Rare, 5, ["sweet"]),
                new OrdinaryTileItem ("Lollipop", Rarity.Common, 3, ["sweet"]),
                t1, t2, t3];
        }

        public GameFlowController? Game { get; private set; }
        public BaseTileItem[] AllTileItemsCollection { get; init; }
        public BaseRelic[] AllRelicsCollection { get; init; }
        public Difficulty[] PossibleDifficulties { get; init; }

        public bool IsGameRunning() => Game is not null;
        public void StartNewGame(GameStarterKit gameStarterKit, Difficulty difficulty)
        {
            Game = new(gameStarterKit, difficulty);
        }

        public void FinishGame()
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

