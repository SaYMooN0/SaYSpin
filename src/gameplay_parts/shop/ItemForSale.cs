using SaYSpin.src.inventory_items;

namespace SaYSpin.src.gameplay_parts.shop
{
    public record class ItemForSale(
        BaseInventoryItem Item,
        int Price
    )
    {
        public bool IsLocked { get; private set; } = false;
        public void Lock() => IsLocked = true;
        public void Unlock() => IsLocked = false;
    }
}