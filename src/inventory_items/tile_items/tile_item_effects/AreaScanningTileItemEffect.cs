

using SaYSpin.src.enums;
using SaYSpin.src.gameplay_parts;
using SaYSpin.src.secondary_classes;

namespace SaYSpin.src.inventory_items.tile_items.tile_item_effects
{
    internal class AreaScanningTileItemEffect : BaseTileItemEffect
    {
        public delegate void AreaScanningDelegate(GameFlowController game, List<TileItemWithCoordinates> tIWithCoordinates);
        public AreaScanningTileItemEffect(string description,TileItem sourceTileItem, SlotMachineArea area, Func<TileItem, bool> condition, AreaScanningDelegate onScannedAction) : base(description , sourceTileItem)
        {
            Area = area;
            Condition = condition;
            PerformOnScannedAction = onScannedAction;
        }
        public SlotMachineArea Area { get; init; }
        public Func<TileItem, bool> Condition { get; init; }
        public AreaScanningDelegate PerformOnScannedAction { get; init; }
    }
}
