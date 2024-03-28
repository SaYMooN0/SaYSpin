using SaYSpin.src.abstract_classes;
using SaYSpin.src.enums;

namespace SaYSpin.src.gameplay_parts.inventory_related.relics
{
    public class RelicWithGameStatInfluence : BaseRelic
    {
        public GameStat GameStat { get; init; }
        public ModifierType ModifierType { get; init; }
        public double ModificationValue { get; init; }
        public RelicWithGameStatInfluence(string name, string description, Rarity rarity, GameStat stat, ModifierType modifierType, double modificationValue)
            : base(name, description, rarity)
        {
            GameStat = stat;
            ModifierType = modifierType;
            ModificationValue = modificationValue;
        }
    }
}
