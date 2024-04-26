using SaYSpin.src.enums;
using SaYSpin.src.inventory_items.tile_items;

namespace SaYSpin.src.tile_item_effects
{
    internal class TileItemsEnhancingTileItemEffect : BaseTileItemEffect
    {
        public TileItemsEnhancingTileItemEffect(string description,TileItem sourceTileItem, SlotMachineArea area, ModifierType modifierType, double modificationValue, Func<TileItem?, bool> condition)
            :base(description, sourceTileItem)
        {
            Area = area;
            ModifierType = modifierType;
            ModificationValue = modificationValue;
            Condition = condition;
        }

        public SlotMachineArea Area { get; init; }
        public ModifierType ModifierType { get; init; }
        public double ModificationValue { get; init; }
        public Func<TileItem?, bool> Condition { get; init; }
    }
}
