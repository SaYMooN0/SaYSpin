using SaYSpin.src.enums;
using SaYSpin.src.inventory_items.relics;
using SaYSpin.src.inventory_items.tile_items;

namespace SaYSpin.src.relic_effects
{
    public class CoinsCalculationEffect:BaseRelicEffect
    {

        public CoinsCalculationEffect(string description, ModifierType modifierType, double modificationValue, Func<BaseTileItem, bool> condition, EffectApplicationArea area) : base(description)
        {
            ModifierType = modifierType;
            ModificationValue = modificationValue;
            Condition = condition;
            Area = area;
        }
        public EffectApplicationArea Area { get; init; }
        public ModifierType ModifierType { get; init; }
        public double ModificationValue { get; init; }
        public Func<BaseTileItem, bool> Condition { get; init; }
    }
}
