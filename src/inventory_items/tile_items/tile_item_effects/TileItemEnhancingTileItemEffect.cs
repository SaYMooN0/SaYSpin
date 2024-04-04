using SaYSpin.src.enums;
using SaYSpin.src.inventory_items.tile_items;

namespace SaYSpin.src.tile_item_effects
{
    internal class TileItemsEnhancingTileItemEffect : BaseTileItemEffect
    {
        public TileItemsEnhancingTileItemEffect(string description, EffectApplicationArea area, ModifierType modifierType, double modificationValue, Func<TileItem, bool> condition):base(description)
        {
            Area = area;
            ModifierType = modifierType;
            ModificationValue = modificationValue;
            Condition = condition;
        }

        public EffectApplicationArea Area { get; init; }
        public ModifierType ModifierType { get; init; }
        public double ModificationValue { get; init; }
        public Func<TileItem, bool> Condition { get; init; }
    }
}
