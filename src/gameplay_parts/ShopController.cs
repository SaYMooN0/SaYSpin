using SaYSpin.src.inventory_items.relics;
using SaYSpin.src.inventory_items.tile_items;

namespace SaYSpin.src.gameplay_parts
{
    public class ShopController
    {
        public ShopController()
        {
            TileItems = [];
            Relics = [];
        }

        public TileItem[] TileItems { get; private set; }
        public Relic[] Relics { get; private set; }

        //special merchant
        public void RefreshShop()
        {

        }
        //available tileItems
        //available relics
        //refresh

    }
}
