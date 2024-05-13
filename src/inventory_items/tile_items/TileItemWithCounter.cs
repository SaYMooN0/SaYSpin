using SaYSpin.src.enums;
using SaYSpin.src.gameplay_parts;

namespace SaYSpin.src.inventory_items.tile_items
{
    public class TileItemWithCounter : TileItem, IWithCounter
    {
        private Func<int> _baseIncomeCalculationFunc;
        private int _counter;
        private TileItemWithCounter(
            string name,
            string description,
            Rarity rarity,
            int initialCoinValue,
            string[] tags,
            bool isSpecial,
            bool isUnique
            ) : base(name, description, rarity, initialCoinValue, tags, new(), null, isSpecial, isUnique) // null because it will be set in the constructor.
        {
            _counter = 0;
            base.CalculateIncome = CalculateIncomeWithCounter;
        }
        public int Counter => _counter;

        public void IncrementCounter(int amount) =>
            _counter += amount;

        public void ResetCounter() =>
            _counter = 0;
        private int CalculateIncomeWithCounter(IEnumerable<TileItemIncomeBonus> bonuses)
        {
            double baseIncome = _baseIncomeCalculationFunc();
            foreach (var bonus in bonuses.OrderByModifierType())
            {
                baseIncome = baseIncome.Apply(bonus.ModifierValue, bonus.ModifierType);
            }
            return (int)baseIncome;
        }
        public TileItemWithCounter WithEffect(BaseTileItemEffect effect) =>
            base.WithEffect(effect) as TileItemWithCounter;
        public TileItemWithCounter SetBaseIncomeCalculationFunc(Func<TileItemWithCounter, int> baseIncomeCalculationFunc)
        {
            _baseIncomeCalculationFunc = () => baseIncomeCalculationFunc(this);
            return this;
        }

        public static TileItemWithCounter New(string name, string description, Rarity rarity, int initialCoinValue, string[] tags, bool isSpecial = false, bool isUnique = false)
        {
            var tileItemWithCounter = new TileItemWithCounter(name, description, rarity, initialCoinValue, tags, isSpecial, isUnique);
            return tileItemWithCounter;
        }
    }
}

