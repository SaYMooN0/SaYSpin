using SaYSpin.src.enums;
using SaYSpin.src.gameplay_parts.game_flow_controller;
using SaYSpin.src.inventory_items.relics;
using SaYSpin.src.inventory_items.relics.relic_effects;

namespace SaYSpin.src.inventory_items.tile_items.tile_item_effects
{
    internal class NonConstantCalculationRelicEffect : BaseRelicEffect
    {
        public delegate Tuple<ModifierType, double> NonConstantCalculationEffectDelegate(GameFlowController game);
        public NonConstantCalculationRelicEffect(string description, NonConstantCalculationEffectDelegate modificationValue, Func<TileItem, bool> condition) : base(description)
        {
            ModificationValue = modificationValue;
            Condition = condition;
        }
        public NonConstantCalculationEffectDelegate ModificationValue { get; init; }
        public Func<TileItem, bool> Condition { get; init; }
        public CoinsCalculationRelicEffect GetCalculationEffect(GameFlowController game)
        {
            (ModifierType type, double val) = ModificationValue(game);
            return new(string.Empty, type, val, Condition);
        }
    }
}
