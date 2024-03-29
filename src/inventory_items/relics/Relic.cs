using SaYSpin.src.gameplay_parts;
using SaYSpin.src.inventory_items;

namespace SaYSpin.src.inventory_items.relics
{
    public class Relic : BaseInventoryItem
    {
        public Relic(string name, Rarity rarity, HashSet<BaseRelicEffect> effects)
            : base(name, string.Join("\n", effects.Select(e => e.Description)), rarity)
        {

            Effects = effects;
        }

        public HashSet<BaseRelicEffect> Effects { get; init; } = new();

        public override string ImageFolderPath => "relics";
    }
}
