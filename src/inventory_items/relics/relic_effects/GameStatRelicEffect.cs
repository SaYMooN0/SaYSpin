using SaYSpin.src.enums;

namespace SaYSpin.src.inventory_items.relics.relic_effects
{
    public class GameStatRelicEffect : BaseRelicEffect
    {
        public GameStat Stat { get; }
        public ModifierType ModifierType { get; }
        public double ModificationValue { get; }

        public GameStatRelicEffect(string description, GameStat stat, ModifierType modifierType, double modificationValue)
            : base(description)
        {
            Stat = stat;
            ModifierType = modifierType;
            ModificationValue = modificationValue;
        }

        public double ApplyEffect(double currentValue)
        {
            switch (ModifierType)
            {
                case ModifierType.Plus:
                    return currentValue + ModificationValue;
                case ModifierType.Multiply:
                    return currentValue * ModificationValue;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ModifierType), "Unsupported modifier type");
            }
        }
    }

}
