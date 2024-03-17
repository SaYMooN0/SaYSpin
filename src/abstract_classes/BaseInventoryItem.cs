using SaYSpin.src.gameplay_parts;

namespace SaYSpin.src.abstract_classes
{
    public abstract class BaseInventoryItem
    {
        public abstract string Id { get; init; }
        public virtual string Image { get; init; } = "default.png";
        public Rarity Rarity { get; init; }
        public abstract string ImageFolderPath { get; }
     
    }
}
