using SaYSpin.src.gameplay_parts;

namespace SaYSpin.src.abstract_classes
{
    public abstract class BaseRelic : BaseInventoryItem
    {
        protected BaseRelic(Rarity rarity)
        {
            Rarity = rarity;
        }

        public override string ImageFolderPath => "relics";
    }
}
