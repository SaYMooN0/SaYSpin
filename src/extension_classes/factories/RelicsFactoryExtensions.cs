using SaYSpin.src.enums;
using SaYSpin.src.inventory_items.relics;
using SaYSpin.src.inventory_items.relics.relic_effects;
using SaYSpin.src.inventory_items.tile_items;
using static SaYSpin.src.inventory_items.relics.relic_effects.AfterSpinRelicEffect;
using static SaYSpin.src.inventory_items.relics.relic_effects.AfterStageCompletedRelicEffect;

namespace SaYSpin.src.extension_classes.factories
{
    internal static class RelicsFactoryExtensions
    {
        public static Relic WithCoinsCalculationRelicEffect(this Relic relic, string description, ModifierType modifierType, int modificationValue, Func<TileItem, bool> condition, EffectApplicationArea area) =>
            relic.WithEffect(new CoinsCalculationEffect(description, modifierType, modificationValue, condition, area));

        public static Relic WithAfterStageCompletedRelicEffect(this Relic relic, string description, AfterStageCompletedAction action) =>
            relic.WithEffect(new AfterStageCompletedRelicEffect(description, action));

        public static Relic WithAfterSpinRelicEffect(this Relic relic, string description, AfterSpinAction action) =>
            relic.WithEffect(new AfterSpinRelicEffect(description, action));
    }
}
