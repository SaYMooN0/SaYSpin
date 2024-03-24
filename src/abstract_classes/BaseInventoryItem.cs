using SaYSpin.src.gameplay_parts;
using System.Globalization;

namespace SaYSpin.src.abstract_classes
{
    public abstract class BaseInventoryItem
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; } = "";
        public string Image { get; init; }
        public Rarity Rarity { get; init; }
        public abstract string ImageFolderPath { get; }
        protected BaseInventoryItem(string name, string description, Rarity rarity)
        {
            Id = name.ToLower().Replace(" ", "_");
            Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToLower());
            Image = Path.Combine(ImageFolderPath, Id + ".png");
            Description = description;
            Rarity = rarity;
        }

    }
}
