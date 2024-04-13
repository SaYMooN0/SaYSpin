using SaYSpin.src.inventory_items.tile_items;
using SaYSpin.src.inventory_items.tile_items.tile_item_effects;

namespace SaYSpin.src.gameplay_parts
{
    static class TileItemsExtensions
    {
        public static bool HasTag(this TileItem? tileItem, string tag) =>
            tileItem is not null && tileItem.Tags.Contains(tag);
        public static bool HasOneOfTags(this TileItem? tileItem, string[] tags) =>
            tileItem is not null && tileItem.Tags.Any(t => tags.Contains(t));
        public static bool IsConsumableByPirate(this TileItem tileItem) =>
            tileItem is not null && tileItem.Tags.Any(t => t == "gold" || t == "chest");
        public static bool IsChest(this TileItem? tileItem) =>
            tileItem.HasTag("chest");
    }
}
