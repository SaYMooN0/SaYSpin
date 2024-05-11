using SaYSpin.src.inventory_items.relics;
using SaYSpin.src.inventory_items.tile_items;

namespace SaYSpin.src.gameplay_parts.shop
{
    public class ShopController
    {
        public ISpecialMerchant[] PossibleSpecialMerchants { get; init; }
        internal ShopController(ISpecialMerchant[] possibleSpecialMerchants)
        {
            this.PossibleSpecialMerchants = possibleSpecialMerchants;
        }
        public List<ItemForSale> LockedItems => TileItemsForSale.Concat(RelicsForSale).Where(i => i.IsLocked).ToList();
        public List<ItemForSale> TileItemsForSale { get; private set; } = [];
        public List<ItemForSale> RelicsForSale { get; private set; } = [];
        public ISpecialMerchant CurrentSpecialMerchant { get; private set; }


        public void Update(List<ItemForSale> newTileItemsForSale, List<ItemForSale> newRelicsForSale)
        {
            TileItemsForSale = newTileItemsForSale;
            RelicsForSale = newRelicsForSale;
        }
        public void UpdateSpecialMerchant()
        {
            ; ; ; ;
        }
        public void ItemBought(ItemForSale itemForSale)
        {
            itemForSale.Unlock();
            if (itemForSale.Item is Relic)
                RelicsForSale.Remove(itemForSale);
            else if (itemForSale.Item is TileItem)
                TileItemsForSale.Remove(itemForSale);

        }

    }
}
