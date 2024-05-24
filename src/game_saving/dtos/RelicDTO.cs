using SaYSpin.src.inventory_items.relics;

namespace SaYSpin.src.game_saving.dtos
{
    internal record class RelicDTO
    {
        public string Name { get; set; }
        public int? Counter { get; set; }
        public static RelicDTO FromRelic(Relic r) =>
             new RelicDTO { Name = r.Name, Counter = (r is RelicWithCounter rWithCounter) ? rWithCounter.Counter : 0 };
        public Relic ToRelic(IDictionary<string, Func<Relic>> relicsConstructors)
        {
            if (!relicsConstructors.ContainsKey(Name))
                throw new ArgumentException($"Constructor for Relic with name '{Name}' not found.");

            Relic r = relicsConstructors[Name]();
            if (r is RelicWithCounter rWithCounter)
            {
                if (Counter is int counter)
                    return RelicWithCounter.WithCounterValue(rWithCounter, counter);
                else
                    throw new ArgumentException("Counter is not an int");
            }
            return r;
        }
    }
}
