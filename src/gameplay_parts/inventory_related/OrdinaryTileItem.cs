using SaYSpin.src.abstract_classes;

namespace SaYSpin.src.gameplay_parts.inventory_related
{
    public class OrdinaryTileItem : BaseTileItem
    {
        public override string Id { get; init; }
        public OrdinaryTileItem(string id, string image, Rarity rarity, int coinValue):base(rarity, coinValue) 
        {
            Id= id;
            Image= image;
        }
    }
}
