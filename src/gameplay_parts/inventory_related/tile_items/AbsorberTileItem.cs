using SaYSpin.src.coins_calculation_related;
using SaYSpin.src.enums;
using SaYSpin.src.inventory_items.tile_items;

namespace SaYSpin.src.gameplay_parts.inventory_related.tile_items
{
    public class AbsorberTileItem : BaseTileItem
    {
        public AbsorberTileItem(string name, string description, Rarity rarity, int initialCoinValue, string[]? tags)
            : base(name, description, rarity, initialCoinValue, tags ?? Array.Empty<string>())
        {
            Counter = 0;
        }
        public int Counter { get; private set; }

        public override int CalculateIncome(IEnumerable<TileItemIncomeBonus> bonuses)
        {
            double value = CalculateBasicCoinValue();
            foreach (var b in bonuses.OrderByModifierType())
            {
                value = value.Apply(b.ModifierValue, b.ModifierType);
            }
            return (int)value;
        }

        public void IncreaseCounter()
        {
            Counter += 1;
        }
        public int CalculateBasicCoinValue()
        {
            return InitialCoinValue + Counter;
        }


    }
}
