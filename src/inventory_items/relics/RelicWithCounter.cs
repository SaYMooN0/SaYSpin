using SaYSpin.src.gameplay_parts;

namespace SaYSpin.src.inventory_items.relics
{
    public class RelicWithCounter : Relic, IWithCounter
    {
        private int _counter;
        public RelicWithCounter(string name, Rarity rarity, int startingCounterValue = 0, bool isSpecial = false, bool isUnique = false) : base(name, rarity, isSpecial, isUnique)
        {
            _counter = startingCounterValue;
        }

        public int Counter => _counter;

        public void IncrementCounter(int amount) =>
            _counter += amount;

        public void ResetCounter() =>
            _counter = 0;
    }
}
