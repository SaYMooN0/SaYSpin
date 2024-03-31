using SaYSpin.src.inventory_items.tile_items;
using SaYSpin.src.inventory_items.tile_items.tile_item_effects;
using static SaYSpin.src.inventory_items.tile_items.tile_item_effects.OnDestroyTileItemEffect;

namespace SaYSpin.src.extension_classes.factories
{
    internal static class TileItemsFactoryExtensions
    {
        public static TileItem WithOnDestroyTileItemEffect(this TileItem tileItem, string description, OnDestroyAction onDestroyAction) =>
            tileItem.WithEffect(new OnDestroyTileItemEffect(description, onDestroyAction));
    }
}
