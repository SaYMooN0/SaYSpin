using SaYSpin.src.enums;
using SaYSpin.src.gameplay_parts.game_flow_controller;

namespace SaYSpin.src.inventory_items.relics.relic_effects
{
    internal class NonConstantGameStatRelicEffect : BaseRelicEffect
    {
        public delegate Tuple<ModifierType, double> NonConstantGameStatRelicEffectDelegate(GameFlowController game);
        public GameStat Stat { get; }
        public NonConstantGameStatRelicEffectDelegate ModificationValue { get; }

        public NonConstantGameStatRelicEffect(string description, GameStat stat, NonConstantGameStatRelicEffectDelegate modificationValue)
            : base(description)
        {
            Stat = stat;
            ModificationValue = modificationValue;
        }

        public double ApplyEffect(double currentValue, GameFlowController game)
        {
            (ModifierType type, double val) = ModificationValue(game);
            switch (type)
            {
                case ModifierType.Plus:
                    return currentValue + val;
                case ModifierType.Multiply:
                    return currentValue * val;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ModifierType), "Unsupported modifier type");
            }
        }
        public GameStatRelicEffect ConvertToStaticEffect(GameFlowController game)
        {
            var (modifierType, value) = ModificationValue(game);
            return new GameStatRelicEffect(Description, Stat, modifierType, value);
        }
    }
}
