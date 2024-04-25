

using SaYSpin.src.enums;
using SaYSpin.src.gameplay_parts;
using SaYSpin.src.secondary_classes;

namespace SaYSpin.src.inventory_items.tile_items.tile_item_effects
{
    internal class AreaScanningTileItemEffect : BaseTileItemEffect
    {
        public AreaScanningTileItemEffect(string description, SlotMachineArea area, Func<TileItem, bool> condition, Action<GameFlowController, List<TileItemWithCoordinates>> onScannedAction) : base(description)
        {
            Area = area;
            Condition = condition;
            PerformOnScannedAction = onScannedAction;
        }
        public SlotMachineArea Area { get; init; }
        public Func<TileItem, bool> Condition { get; init; }
        public Action<GameFlowController, List<TileItemWithCoordinates>> PerformOnScannedAction { get; init; }
    }
}
