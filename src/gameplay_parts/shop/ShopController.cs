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
        public ItemForSale[] TileItemsForSale { get; private set; } = [];
        public ItemForSale[] RelicsForSale { get; private set; } = [];
        public ISpecialMerchant CurrentSpecialMerchant { get; private set; }


        public void Update(ItemForSale[] newTileItemsForSale, ItemForSale[] newRelicsForSale)
        {
            TileItemsForSale = newTileItemsForSale;
            RelicsForSale = newRelicsForSale;
        }
        public void UpdateSpecialMerchant()
        {
            ; ; ; ;
        }

    }
}
