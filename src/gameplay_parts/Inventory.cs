using SaYSpin.src.abstract_classes;
using SaYSpin.src.gameplay_parts.inventory_related.tokens;

namespace SaYSpin.src.gameplay_parts
{
    public class Inventory
    {
        public Inventory(List<BaseTileItem> startingItems, List<BaseRelic>? startingRelics)
        {
            Tokens = new();
            Relics = startingRelics is null ? new List<BaseRelic>() : startingRelics;
            TileItems = startingItems;
            DiamondsCount = 5;
        }
        public int DiamondsCount { get; private set; }
        public TokensCollection Tokens { get; init; }
        public List<BaseRelic> Relics { get; init; }
        public List<BaseTileItem> TileItems { get; init; }
        public void AddTileItem(BaseTileItem item) =>
            TileItems.Add(item);
        public void AddRelic(BaseRelic relic) =>
            Relics.Add(relic);
    }
}
