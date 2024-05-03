using SaYSpin.src.inventory_items.tile_items;
using SaYSpin.src.inventory_items.relics;
using SaYSpin.src.inventory_items;
using SaYSpin.src.gameplay_parts.shop;
using SaYSpin.src.static_classes;
namespace SaYSpin.src.gameplay_parts.game_flow_controller
{
    public partial class GameFlowController
    {
        private bool _possibleRelicsReinitNeeded = true;
        private Relic[] _possibleToDropRelics;
        public IEnumerable<Relic> RelicsPossibleToDrop
        {
            get
            {
                if (_possibleRelicsReinitNeeded)
                {
                    var takenUniqueRelics = Inventory.Relics.Where(r => r.IsUnique);
                    _possibleToDropRelics = AllRelicsCollection
                        .Where(r => r.IsSpecial)
                        .Except(takenUniqueRelics)
                        .ToArray();
                    _possibleRelicsReinitNeeded = false;
                }
                return _possibleToDropRelics;
            }
        }

        private bool _possibleTileItemsReinitNeeded = true;
        private TileItem[] _possibleToDropTileItems;
        public IEnumerable<TileItem> TileItemsPossibleToDrop
        {
            get
            {
                if (_possibleTileItemsReinitNeeded)
                {
                    var takenUniqueTileItems = Inventory.TileItems.Where(ti => ti.IsUnique);
                    _possibleToDropTileItems = AllTileItemsCollection
                        .Where(ti => ti.IsSpecial)
                        .Except(takenUniqueTileItems)
                        .ToArray();
                    _possibleTileItemsReinitNeeded = false;
                }
                return _possibleToDropTileItems;
            }
        }

        private void SetPossibleInventoryItemsReinitNeeded(BaseInventoryItem item)
        {
            if (item is Relic)
                _possibleRelicsReinitNeeded = true;
            if (item is TileItem)
                _possibleTileItemsReinitNeeded = true;
        }
        public List<GameStarterKit> GenerateStarterKits()
        {
            List<GameStarterKit> kits = new();
            var commonItems = AllTileItemsCollection.Where(i => i.Rarity == Rarity.Common && !i.IsSpecial).OrderBy(x => Guid.NewGuid()).ToList();
            var rareItems = AllTileItemsCollection.Where(i => i.Rarity == Rarity.Rare && !i.IsSpecial).OrderBy(x => Guid.NewGuid()).ToList();
            var relics = AllRelicsCollection.Where(r => r.Rarity <= Rarity.Rare && !r.IsSpecial).OrderBy(x => Guid.NewGuid()).ToList();

            int totalKits = 4;
            int commonItemsPerKit = Difficulty.StartingTileItemsCount;
            int relicsPerKit = Difficulty.StartingRelicsCount;

            for (int i = 0; i < totalKits; i++)
            {
                List<TileItem> itemsForKit = new();
                for (int j = 0; j < commonItemsPerKit; j++)
                {
                    itemsForKit.Add(commonItems[(i * commonItemsPerKit + j) % commonItems.Count]);
                }
                itemsForKit.Add(rareItems[i % rareItems.Count]);

                List<Relic> relicsForKit = new();
                for (int j = 0; j < relicsPerKit; j++)
                {
                    relicsForKit.Add(relics[(i * relicsPerKit + j) % relics.Count]);
                }

                kits.Add(new GameStarterKit(itemsForKit, relicsForKit,
                    GameStarterKit.RandomTokensCollection(Difficulty.StartingTokensCount), Difficulty.StartingDiamondsCount));
            }

            return kits;
        }
        public TileItem[] GenerateTileItemsForNewStageChoosing()
        {

            return TileItemsPossibleToDrop.OrderBy(x => Guid.NewGuid()).Take(StatsTracker.NewStageTileItemsForChoiceCount).ToArray();
            //will be changed  based on luck
        }
        public Relic[] GenerateRelicsForNewStageChoosing()
        {
            return RelicsPossibleToDrop.OrderBy(x => Guid.NewGuid()).Take(StatsTracker.NewStageRelicsForChoiceCount).ToArray();
            //will be changed based on luck
        }
        private void UpdateShopItems()
        {
            double luckParam = CalculateCurrentLuckPoints();
            var tileItems = GenerateNewTileItemsForShop(luckParam);
            var relics = GenerateNewRelicsForShop(luckParam);
            //will be changed based on rarity
            Shop.Update(
                tileItems.Select(ti => new ItemForSale<TileItem>(ti, Randomizer.Int(20, 100))).ToArray(),
                relics.Select(r => new ItemForSale<Relic>(r, Randomizer.Int(20, 100))).ToArray()
                );
        }
        private double CalculateCurrentLuckPoints() =>
            CurrentStage % 10 == 0 ? StatsTracker.Luck + 2
            : CurrentStage % 5 == 0 ? StatsTracker.Luck + 1
            : StatsTracker.Luck;

        private TileItem[] GenerateNewTileItemsForShop(double currentLuckPoints)
        {
            Random rnd = new();

            TileItem[] tileItemsToReturn = new TileItem[StatsTracker.TileItemsInShopCount];
            var tileItems = TileItemsPossibleToDrop.ToArray();


            for (int i = 0; i < StatsTracker.TileItemsInShopCount; i++)
            {
                int index = rnd.Next(tileItems.Length);
                tileItemsToReturn[i] = tileItems[index];
            }
            return tileItemsToReturn;
        }
        private Relic[] GenerateNewRelicsForShop(double currentLuckPoints)
        {
            Random rnd = new();

            Relic[] relicsToReturn = new Relic[StatsTracker.RelicsInShopCount];
            var relics = RelicsPossibleToDrop.ToArray();


            for (int i = 0; i < StatsTracker.RelicsInShopCount; i++)
            {
                int index = rnd.Next(relics.Length);
                relicsToReturn[i] = relics[index];
            }
            return relicsToReturn;
        }

    }
}
