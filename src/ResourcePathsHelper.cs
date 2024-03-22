namespace SaYSpin.src
{
    public static class ResourcePathsHelper
    {
        public const string
            DefaultRelic = "/resources/images/relics/default.png",
            DefaultTileItem = "/resources/images/tile_items/default.png";
        public static string ResourcesImages(string fileName) =>
            "/resources/images/" + fileName;
        public static string RelicPath(string fileName) =>
            "/resources/images/relics/" + fileName;
        public static string TileItemPath(string fileName) =>
            "/resources/images/tile_items/" + fileName;
    }
}
