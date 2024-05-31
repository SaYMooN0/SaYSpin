using SaYSpin.src.gameplay_parts;

namespace SaYSpin.src.inventory_items.relics
{
    public class RelicWithCounter : Relic, IWithCounter
    {
        private int _counter;
        public RelicWithCounter(string name, Rarity rarity, bool isSpecial = false, bool isUnique = false, int startingCounterValue = 0) : base(name, rarity, isSpecial, isUnique)
        {
            _counter = startingCounterValue;
        }

        public int Counter => _counter;

        public void IncrementCounter(int amount) =>
            _counter += amount;

        public void ResetCounter() =>
            _counter = 0;
        public static RelicWithCounter WithCounterValue(RelicWithCounter baseObj, int value)
        {
            RelicWithCounter newR = new(baseObj.Name, baseObj.Rarity, baseObj.IsSpecial, baseObj.IsUnique, value);
            foreach (var effect in baseObj.Effects)
                newR.WithEffect(effect);
            return newR;
        }
    }
}
