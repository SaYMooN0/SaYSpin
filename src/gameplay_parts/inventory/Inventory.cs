using SaYSpin.src.gameplay_parts.inventory_related.tokens;
using SaYSpin.src.gameplay_parts.shop;
using SaYSpin.src.inventory_items.relics;
using SaYSpin.src.inventory_items.tile_items;

namespace SaYSpin.src.gameplay_parts.inventory_related
{
    public class Inventory
    {
        public Inventory(List<TileItem> tileItems, List<Relic> relics, TokensCollection tokens, int diamondsCount)
        {
            Tokens = tokens;
            Relics = relics;
            TileItems = tileItems;
            DiamondsCount = diamondsCount;
        }
        public int DiamondsCount { get; private set; }
        public TokensCollection Tokens { get; init; }
        public List<Relic> Relics { get; init; }
        public List<TileItem> TileItems { get; init; }
        public void ChangeDiamondsCount(Func<int, int> func) =>
            DiamondsCount = func(DiamondsCount);
        public void AddDiamonds(int count) =>
            DiamondsCount += count;

        public (bool ableToBuy, string? errorMessage) IsAbleToBuy(ItemForSale itemToBuy)
        {
            if (itemToBuy.Price > DiamondsCount)
                return (false, "Not enough diamonds");
            else if (itemToBuy.Item.IsUnique)
            {
                if (itemToBuy.Item is Relic r)
                    if (Relics.Any(relic => relic.Id == r.Id))
                        return (false, "You already have this unique relic");

                if (itemToBuy.Item is TileItem ti)
                    if (TileItems.Any(tileItem => tileItem.Id == ti.Id))
                        return (false, "You already have this unique tile item");
            }
            return (true, null);
        }
    }
}
