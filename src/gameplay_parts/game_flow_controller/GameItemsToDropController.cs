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
                    var takenUniqueRelicsNames = Inventory.Relics.Where(r => r.IsUnique).Select(r => r.Name);
                    _possibleToDropRelics = AllRelicsCollection
                        .Where(r => !r.IsSpecial)
                        .Where(r => !takenUniqueRelicsNames.Contains(r.Name))
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
                    var takenUniqueTileItemsNames = Inventory.TileItems.Where(ti => ti.IsUnique).Select(ti => ti.Name);
                    _possibleToDropTileItems = AllTileItemsCollection
                        .Where(ti => !ti.IsSpecial)
                        .Where(ti => !takenUniqueTileItemsNames.Contains(ti.Name))
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
     
        public TileItem[] GenerateTileItemsForNewStageChoosing()
        {

            return TileItemsPossibleToDrop.OrderBy(x => Guid.NewGuid()).Take(StatsTracker.NewStageTileItemsForChoiceCount).ToArray();
            //TODO change  based on luck
        }
        public Relic[] GenerateRelicsForNewStageChoosing()
        {
            return RelicsPossibleToDrop.OrderBy(x => Guid.NewGuid()).Take(StatsTracker.NewStageRelicsForChoiceCount).ToArray();
            //TODO change  based on luck
        }
        internal void UpdateShopItems()
        {
            double luckParam = CalculateCurrentLuckPoints();

            var lockedTileItems = Shop.TileItemsForSale.Where(ti => ti.IsLocked);
            var lockedRelics = Shop.RelicsForSale.Where(r => r.IsLocked);


            int tileItemsNeeded = StatsTracker.TileItemsInShopCount - lockedTileItems.Count();
            int relicsNeeded = StatsTracker.RelicsInShopCount - lockedRelics.Count();

            var tileItems = tileItemsNeeded > 0 ? GenerateNewTileItemsForShop(luckParam, tileItemsNeeded) : [];
            var relics = relicsNeeded > 0 ? GenerateNewRelicsForShop(luckParam, relicsNeeded) : [];
            //TODO change  based on rarity

            Shop.Update(
                lockedTileItems.Concat(tileItems).ToList(),
                lockedRelics.Concat(relics).ToList());
        }
        private double CalculateCurrentLuckPoints() =>
            CurrentStage % 10 == 0 ? StatsTracker.Luck + 2
            : CurrentStage % 5 == 0 ? StatsTracker.Luck + 1
            : StatsTracker.Luck;

        private IEnumerable<ItemForSale> GenerateNewTileItemsForShop(double currentLuckPoints, int tileItemsNeeded)
        {
            Random rnd = new();

            TileItem[] tileItemsToReturn = new TileItem[tileItemsNeeded];
            var tileItems = TileItemsPossibleToDrop.ToArray();


            for (int i = 0; i < tileItemsNeeded; i++)
            {
                int index = rnd.Next(tileItems.Length);
                tileItemsToReturn[i] = tileItems[index];
            }
            return AddPriceToItems(tileItemsToReturn);
        }
        private IEnumerable<ItemForSale> GenerateNewRelicsForShop(double currentLuckPoints, int relicsNeeded)
        {
            Random rnd = new();

            Relic[] relicsToReturn = new Relic[relicsNeeded];
            var relics = RelicsPossibleToDrop.ToArray();


            for (int i = 0; i < relicsNeeded; i++)
            {
                int index = rnd.Next(relics.Length);
                relicsToReturn[i] = relics[index];
            }
            return AddPriceToItems(relicsToReturn);
        }
        private IEnumerable<ItemForSale> AddPriceToItems(BaseInventoryItem[] items) =>
            items.Select(
                item => new ItemForSale(
                    item,
                    (int)((item.Rarity.ItemPrice() * (CurrentStage + 2) / 3 + 7) * StatsTracker.ShopPriceCoefficient) + Randomizer.Int(0, CurrentStage / 3 + 3)
                )
            );
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
    }
}
