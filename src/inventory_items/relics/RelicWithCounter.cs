using SaYSpin.src.gameplay_parts;

namespace SaYSpin.src.inventory_items.relics
{
    public class RelicWithCounter : Relic, IWithCounter
    {
        private int _counter;
        public RelicWithCounter(string name, Rarity rarity) : base(name, rarity)
        {
            _counter = 0;
        }

        public int Counter => _counter;

        public void IncrementCounter(int amount) =>
            _counter += amount;

        public void ResetCounter() =>
            _counter = 0;
    }
}
