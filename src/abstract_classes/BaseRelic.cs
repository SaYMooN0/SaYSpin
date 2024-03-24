using SaYSpin.src.gameplay_parts;

namespace SaYSpin.src.abstract_classes
{
    public abstract class BaseRelic : BaseInventoryItem
    {
        public BaseRelic(string name, string description, Rarity rarity)
            : base(name, description, rarity) { }
        public override string ImageFolderPath => "relics";
    }
}
