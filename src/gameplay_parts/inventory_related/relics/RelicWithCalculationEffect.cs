using SaYSpin.src.abstract_classes;
using SaYSpin.src.coins_calculation_related;

namespace SaYSpin.src.gameplay_parts.inventory_related.relics
{
    public class RelicWithCalculationEffect : BaseRelic
    {
        public RelicWithCalculationEffect(string id, string name, string description, string image, Rarity rarity, CalculationEffect effect)
            : base(id, name, description, image, rarity)
        {
        }
    }
}
