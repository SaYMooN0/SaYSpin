using SaYSpin.src.inventory_items;

namespace SaYSpin.src.gameplay_parts.shop
{
    public class ItemForSale<T> where T : BaseInventoryItem
    {
        public T Item { get; init; }
        public int Price { get; init; }
        public ItemForSale(T item, int price)
        {
            Item = item;
            Price = price;
        }
    }
}
