using SaYSpin.src.enums;
using SaYSpin.src.inventory_items.tile_items;

namespace SaYSpin.src.inventory_items.relics.relic_effects
{
    public class CoinsCalculationEffect : BaseRelicEffect
    {

        public CoinsCalculationEffect(string description, ModifierType modifierType, double modificationValue, Func<TileItem, bool> condition, EffectApplicationArea area) : base(description)
        {
            ModifierType = modifierType;
            ModificationValue = modificationValue;
            Condition = condition;
            Area = area;
        }
        public EffectApplicationArea Area { get; init; }
        public ModifierType ModifierType { get; init; }
        public double ModificationValue { get; init; }
        public Func<TileItem, bool> Condition { get; init; }
    }
}
