using SaYSpin.src.enums;
using SaYSpin.src.inventory_items.relics;
using SaYSpin.src.inventory_items.relics.relic_effects;
using SaYSpin.src.inventory_items.tile_items;
using SaYSpin.src.inventory_items.tile_items.tile_item_effects;
using System;
using static SaYSpin.src.inventory_items.relics.relic_effects.AfterSpinRelicEffect;
using static SaYSpin.src.inventory_items.relics.relic_effects.AfterStageRewardRelicEffect;
using static SaYSpin.src.inventory_items.relics.relic_effects.AfterTokenUsedRelicEffect;
using static SaYSpin.src.inventory_items.relics.relic_effects.OnStageStartedRelicEffect;
using static SaYSpin.src.inventory_items.tile_items.tile_item_effects.NonConstantCalculationRelicEffect;

namespace SaYSpin.src.extension_classes.factories
{
    internal static class RelicsFactoryExtensions
    {
        public static Relic WithCoinsCalculationRelicEffect(this Relic relic, string description, ModifierType modifierType, int modificationValue, Func<TileItem, bool> condition) =>
            relic.WithEffect(new CoinsCalculationRelicEffect(description, modifierType, modificationValue, condition));
        public static Relic WithNonConstantCalculationRelicEffect(this Relic relic, string description, NonConstantCalculationEffectDelegate modificationValue, Func<TileItem, bool> condition) =>
            relic.WithEffect(new NonConstantCalculationRelicEffect(description, modificationValue, condition));

        public static Relic WithAfterStageRewardRelicEffect(this Relic relic, string description, AfterStageCompletedReward reward) =>
            relic.WithEffect(new AfterStageRewardRelicEffect(description, reward));

        public static Relic WithAfterSpinRelicEffect(this Relic relic, string description, AfterSpinAction action) =>
            relic.WithEffect(new AfterSpinRelicEffect(description, action));

        public static Relic WithOnStageStartedRelicEffect(this Relic relic, string description, OnStageStartedAction action) =>
            relic.WithEffect(new OnStageStartedRelicEffect(description, action));
        public static Relic WithAfterTokenUsedRelicEffect(this Relic relic, string description, AfterTokenUsedAction action) =>
            relic.WithEffect(new AfterTokenUsedRelicEffect(description, action));

        public static Relic WithGameStatRelicEffect(this Relic relic, string description, GameStat gameStat, ModifierType modifierType, double modificationValue) =>
            relic.WithEffect(new GameStatRelicEffect(description, gameStat, modifierType, modificationValue));

    }
}
