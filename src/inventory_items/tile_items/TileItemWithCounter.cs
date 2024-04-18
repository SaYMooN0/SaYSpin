using SaYSpin.src.enums;
using SaYSpin.src.gameplay_parts;

namespace SaYSpin.src.inventory_items.tile_items.tile_item_with_counter
{

    namespace SaYSpin.src.inventory_items.tile_items.tile_item_with_counter
    {
        public class TileItemWithCounter : TileItem
        {
            private readonly Func<int> _baseIncomeCalculationFunc;

            public TileItemWithCounter(
                string name,
                string description,
                Rarity rarity,
                int initialCoinValue,
                string[] tags,
                Func<int> baseIncomeCalculationFunc)
                : base(name, description, rarity, initialCoinValue, tags, new(), null) // null because it will be set in the constructor.
            {
                Counter = 0;
                _baseIncomeCalculationFunc = baseIncomeCalculationFunc;
                CalculateIncome = CalculateIncomeWithCounter;
            }

            public int Counter { get; private set; }

            public void IncrementCounter(int amount)=>
                Counter += amount;

            public void ResetCounter()=>
                Counter = 0;

            private int CalculateIncomeWithCounter(IEnumerable<TileItemIncomeBonus> bonuses)
            {
                double baseIncome = _baseIncomeCalculationFunc();  
                foreach (var bonus in bonuses.OrderByModifierType())
                {
                    baseIncome = baseIncome.Apply(bonus.ModifierValue, bonus.ModifierType);
                }
                return (int)baseIncome;
            }
        }
    }
}
