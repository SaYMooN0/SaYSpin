using SaYSpin.src.gameplay_parts;

namespace SaYSpin.src.inventory_items.relics
{
    public class Relic : BaseInventoryItem
    {
        public HashSet<BaseRelicEffect> Effects { get; init; } = new HashSet<BaseRelicEffect>();

        public Relic(string name, Rarity rarity, IEnumerable<BaseRelicEffect> effects = null)
            : base(name, effects != null ? string.Join("\n", effects.Select(e => e.Description)) : string.Empty, rarity)
        {
            if (effects is not null)
            {
                foreach (var effect in effects)
                {
                    Effects.Add(effect);
                }
            }
        }

        public Relic WithEffect(BaseRelicEffect effect)
        {
            Effects.Add(effect);
            Description += (string.IsNullOrEmpty(Description) ? "" : "\n") + effect.Description;
            return this;
        }
        public override string ImageFolderPath => "relics";
    }

}
