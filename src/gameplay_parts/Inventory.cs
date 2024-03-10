using SaYSpin.src.abstract_classes;
using SaYSpin.src.gameplay_parts.inventory_related;

namespace SaYSpin.src.gameplay_parts
{
    public class Inventory
    {
        public Inventory()
        {
            Tokens = new();
            Relics = new();
            TileItems = new();
        }

        public TokensCollection Tokens { get; init; }
        public List<BaseRelic> Relics { get; init; }
        public List<BaseTileItem> TileItems { get; init; }
    }
}
