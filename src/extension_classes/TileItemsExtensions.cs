using SaYSpin.src.inventory_items.tile_items;

namespace SaYSpin.src.extension_classes
{
    static class TileItemsExtensions
    {
        public static bool HasTag(this TileItem? tileItem, string tag) =>
            tileItem is not null && tileItem.Tags.Contains(tag);
        public static bool HasOneOfTags(this TileItem? tileItem,params string[] tags) =>
            tileItem is not null && tileItem.Tags.Any(t => tags.Contains(t));
        public static bool IsConsumableByPirate(this TileItem tileItem) =>
            tileItem is not null && tileItem.Tags.Any(t => t == "gold" || t == "chest");
        public static bool IsChest(this TileItem? tileItem) =>
            tileItem.HasTag("chest");
        public static bool IsAlien(this TileItem? tileItem) =>
            tileItem.HasTag("alien");
        public static bool IsPlanet(this TileItem? tileItem) =>
            tileItem.HasTag("planet");
        public static bool IdIs(this TileItem? tileItem, string id) =>
            tileItem is not null && tileItem.Id == id;
    }
}
