using SaYSpin.src.abstract_classes;

namespace SaYSpin.src.gameplay_parts.inventory_related.tile_items
{
    public class OrdinaryTileItem : BaseTileItem
    {
        public OrdinaryTileItem(string idPostfix, string name, string image, Rarity rarity, int coinValue, string[]? tags)
            : base($"item:{idPostfix}", name, $"Gives {coinValue} coins with each drop", image, rarity, coinValue, tags ?? Array.Empty<string>()) { }

    }
}
