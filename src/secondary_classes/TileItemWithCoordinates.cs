using SaYSpin.src.inventory_items.tile_items;

namespace SaYSpin.src.secondary_classes
{
    public record class TileItemWithCoordinates(TileItem TileItem, int row, int column)
    {
    }
}
