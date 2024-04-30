using SaYSpin.src.gameplay_parts.inventory_related;
using SaYSpin.src.secondary_classes;
using SaYSpin.src.extension_classes;
using SaYSpin.src.inventory_items.tile_items;
using SaYSpin.src.inventory_items.relics;
using SaYSpin.src.inventory_items.relics.relic_effects;
using SaYSpin.src.inventory_items;
using SaYSpin.src.gameplay_parts.inventory_related.tokens;
namespace SaYSpin.src.gameplay_parts.game_flow_controller
{
    public partial class GameFlowController
    {
        private IEnumerable<TileItem> ShopAndRewardsPossibleTileItems()
        {
            //will be changed
            return TileItems;
        }
        private IEnumerable<Relic> ShopAndRewardsPossibleRelics()
        {
            //will be changed
            return Relics;
        }
        public List<GameStarterKit> GenerateStarterKits()
        {
            List<GameStarterKit> kits = new();
            var commonItems = TileItems.Where(i => i.Rarity == Rarity.Common).OrderBy(x => Guid.NewGuid()).ToList();
            var rareItems = TileItems.Where(i => i.Rarity == Rarity.Rare).OrderBy(x => Guid.NewGuid()).ToList();
            var relics = Relics.Where(r => r.Rarity <= Rarity.Rare).OrderBy(x => Guid.NewGuid()).ToList();

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

            return TileItems.OrderBy(x => Guid.NewGuid()).Take(StatsTracker.NewStageTileItemsForChoiceCount).ToArray();
            //will be changed
        }
        public Relic[] GenerateRelicsForNewStageChoosing()
        {
            return Relics.OrderBy(x => Guid.NewGuid()).Take(StatsTracker.NewStageRelicsForChoiceCount).ToArray();
            //will be changed
        }
    }
}
