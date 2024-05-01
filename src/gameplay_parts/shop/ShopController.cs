using SaYSpin.src.inventory_items.relics;
using SaYSpin.src.inventory_items.tile_items;

namespace SaYSpin.src.gameplay_parts.shop
{
    public  class ShopController
    {
        private readonly ISpecialMerchant[] possibleSpecialMerchants;
        internal ShopController(ISpecialMerchant[] possibleSpecialMerchants)
        {
            this.possibleSpecialMerchants = possibleSpecialMerchants;
        }

        public ItemForSale<TileItem>[] TileItemsForSale { get; private set; }
        public ItemForSale<Relic>[] RelicsForSale {  get; private set; }
        public ISpecialMerchant CurrentSpecialMerchant { get; private set; }
      

        public void Update(ItemForSale<TileItem>[] newTileItemsForSale, ItemForSale<Relic>[] newRelicsForSale)
        {
            TileItemsForSale=newTileItemsForSale;
            RelicsForSale = newRelicsForSale;
        }
        public void UpdateSpecialMerchant()
        {
            ; ; ; ;
        }

    }
}
