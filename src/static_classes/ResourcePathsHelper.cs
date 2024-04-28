namespace SaYSpin.src
{
    public static class ResourcePathsHelper
    {
        public const string
            DefaultRelic = "/resources/images/relics/default.png",
            DefaultTileItem = "/resources/images/tile_items/default.png";
        public static string ResourcesImages(string fileName) =>
            "/resources/images/" + fileName;
    }
}
