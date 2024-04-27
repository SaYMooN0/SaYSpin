using SaYSpin.src.gameplay_parts;
using System.Text;

namespace SaYSpin.src.inventory_items.relics
{
    public class Relic : BaseInventoryItem
    {
        public HashSet<BaseRelicEffect> Effects { get; init; } = new HashSet<BaseRelicEffect>();
        public override string ImageFolderPath => "relics";
        public Relic(string name, Rarity rarity, bool isAvailableInBeforeStageChoosingPhase =true, bool isUnique=false) : base(name, string.Empty, rarity, isAvailableInBeforeStageChoosingPhase, isUnique) { }

        public Relic WithEffect(BaseRelicEffect effect)
        {
            Effects.Add(effect);
            Description += (string.IsNullOrEmpty(Description) ? "" : "\n") + effect.Description;
            return this;
        }

        public override string TextInfo()
        {
            StringBuilder sb = new();
            sb.Append($"Name: {Name}\nRarity: {Rarity}\nId: {Id}\n");
            if (Effects.Count < 1)
            {
                sb.Append("No effects");
                return sb.ToString();
            }
            sb.Append("Effects:\n");
            for (int i = 0; i < Effects.Count; i++)
            {
                sb.Append($"{i + 1}. {Effects.ElementAt(i).Description}\n");
            }
            return sb.ToString();
        }


    }

}
