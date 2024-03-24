using SaYSpin.src.abstract_classes;
using SaYSpin.src.coins_calculation_related;

namespace SaYSpin.src.gameplay_parts.inventory_related.relics
{
    public class RelicWithCalculationEffect : BaseRelic
    {
        public RelicWithCalculationEffect(string name, string description, Rarity rarity, CalculationEffect effect)
            : base(name, description, rarity)
        {
        }
    }
}
