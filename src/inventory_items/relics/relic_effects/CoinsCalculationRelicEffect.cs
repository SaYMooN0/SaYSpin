using SaYSpin.src.enums;
using SaYSpin.src.inventory_items.tile_items;

namespace SaYSpin.src.inventory_items.relics.relic_effects
{
    public class CoinsCalculationRelicEffect : BaseRelicEffect
    {

        public CoinsCalculationRelicEffect(string description, ModifierType modifierType, double modificationValue, Func<TileItem, bool> condition) : base(description)
        {
            ModifierType = modifierType;
            ModificationValue = modificationValue;
            Condition = condition;
        }
        public ModifierType ModifierType { get; init; }
        public double ModificationValue { get; init; }
        public Func<TileItem, bool> Condition { get; init; }
    }
}
