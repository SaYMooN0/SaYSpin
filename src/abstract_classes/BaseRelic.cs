using SaYSpin.src.gameplay_parts;

namespace SaYSpin.src.abstract_classes
{
    public abstract class BaseRelic : BaseInventoryItem
    {
        public BaseRelic(string id, string name, string description, string image, Rarity rarity)
            : base(id, name, description, image, rarity) { }

        public override string ImageFolderPath => "relics";
    }
}
