using SaYSpin.src.abstract_classes;
using SaYSpin.src.gameplay_parts.inventory_related;

namespace SaYSpin.src.gameplay_parts
{
    public class Inventory
    {
        public Inventory(List<BaseTileItem> startingItems, List<BaseRelic>? startingRelics)
        {
            Tokens = new();
            Relics = startingRelics is null? new List<BaseRelic>() : startingRelics;
            TileItems = startingItems;
        }

        public TokensCollection Tokens { get; init; }
        public List<BaseRelic> Relics { get; init; }
        public List<BaseTileItem> TileItems { get; init; }
    }
}
