using SaYSpin.src.abstract_classes;

namespace SaYSpin.src.gameplay_parts.inventory_related.tile_items
{
    public class OrdinaryTileItem : BaseTileItem
    {
        public OrdinaryTileItem(string id, string name, Rarity rarity, int coinValue, string[]? tags)
            : base(id, name, $"Gives {coinValue} coins with each drop", rarity, coinValue, tags ?? Array.Empty<string>()) { }

    }
}
