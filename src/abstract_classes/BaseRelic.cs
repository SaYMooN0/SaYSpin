using SaYSpin.src.gameplay_parts;

namespace SaYSpin.src.abstract_classes
{
    public abstract class BaseRelic : BaseInventoryItem
    {
        public BaseRelic(string id, string name, string description, Rarity rarity)
            : base(id, name, description, rarity) { }
        public override string ImageFolderPath => "relics";
    }
}
