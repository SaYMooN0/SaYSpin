using SaYSpin.src.gameplay_parts;

namespace SaYSpin.src.abstract_classes
{
    public abstract class BaseRelic : BaseInventoryItem
    {
        public BaseRelic(string idPostfix, string name, string description, string image, Rarity rarity)
            : base($"relic:{idPostfix}", name, description, image, rarity) { }
        public override string ImageFolderPath => "relics";
    }
}
