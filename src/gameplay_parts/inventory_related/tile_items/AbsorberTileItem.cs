using SaYSpin.src.abstract_classes;

namespace SaYSpin.src.gameplay_parts.inventory_related.tile_items
{
    public class AbsorberTileItem : BaseTileItem
    {
        public AbsorberTileItem(string id, string name, Rarity rarity, int coinValue, string[]? tags)
            : base(name, $"Gives {coinValue} coins with each drop", rarity, coinValue, tags ?? Array.Empty<string>()) { }
        public int Counter { get;private set; }

    }
}
