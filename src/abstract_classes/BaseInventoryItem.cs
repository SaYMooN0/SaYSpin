using SaYSpin.src.gameplay_parts;

namespace SaYSpin.src.abstract_classes
{
    public abstract class BaseInventoryItem
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; } = "";
        public virtual string Image { get; init; } = "default.png";
        public Rarity Rarity { get; init; }
        public abstract string ImageFolderPath { get; }
        protected BaseInventoryItem(string id, string name, string description, string image, Rarity rarity)
        {
            Id = id;
            Name = name;
            Description = description;
            Image = image;
            Rarity = rarity;
        }
    }
}
