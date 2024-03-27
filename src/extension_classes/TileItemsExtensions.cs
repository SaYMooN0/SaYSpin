using SaYSpin.src.abstract_classes;

namespace SaYSpin.src.extension_classes
{
    static class TileItemsExtensions
    {
        public static bool HasTag(this BaseTileItem tileItem, string tag)
        {
            return tileItem.Tags.Contains(tag);
        }
        public static bool IsConsumableByPirate(this BaseTileItem tileItem) =>
            tileItem.Tags.Any(t => t == "gold" || t == "chest");
    }
}
