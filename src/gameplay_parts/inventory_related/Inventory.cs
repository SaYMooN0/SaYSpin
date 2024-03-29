using SaYSpin.src.gameplay_parts.inventory_related.tokens;
using SaYSpin.src.inventory_items.relics;
using SaYSpin.src.inventory_items.tile_items;

namespace SaYSpin.src.gameplay_parts.inventory_related
{
    public class Inventory
    {
        public Inventory(List<BaseTileItem> startingItems, List<Relic> startingRelics, TokensCollection tokens, int diamondsCount)
        {
            Tokens = tokens;
            Relics = startingRelics;
            TileItems = startingItems;
            DiamondsCount = diamondsCount;
        }
        public int DiamondsCount { get; private set; }
        public TokensCollection Tokens { get; init; }
        public List<Relic> Relics { get; init; }
        public List<BaseTileItem> TileItems { get; init; }
        public void AddTileItem(BaseTileItem item) =>
            TileItems.Add(item);
        public void AddRelic(Relic relic) =>
            Relics.Add(relic);

        public void IncreaseDiamonds(int value) =>
            DiamondsCount += value;
        public void DecreaseDiamonds(int value) =>
            DiamondsCount -= value;
    }
}
