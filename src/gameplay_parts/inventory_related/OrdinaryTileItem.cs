using SaYSpin.src.abstract_classes;

namespace SaYSpin.src.gameplay_parts.inventory_related
{
    public class OrdinaryTileItem : BaseTileItem
    {
        public OrdinaryTileItem(string idPostfix, string name, string image, Rarity rarity, int coinValue)
            : base($"item:{idPostfix}", name, $"Gives {coinValue} coins with each drop", image, rarity, coinValue) { }

    }
}
