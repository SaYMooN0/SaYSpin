using SaYSpin.src.gameplay_parts;
using System.Globalization;

namespace SaYSpin.src.inventory_items
{
    public abstract class BaseInventoryItem
    {
        public string Id { get; init; }
        public string Name { get; init; }
        public string Description { get; protected set; } = "";
        public string Image { get; init; }
        public Rarity Rarity { get; init; }
        public bool IsUnique{get;init;}
        public bool ObtainableInRegularCases{get;init;}
        public abstract string ImageFolderPath { get; }
        protected BaseInventoryItem(string name, string description, Rarity rarity, bool isAvailableInBeforeStageChoosingPhase=true, bool isUnique=false)
        {
            Id = name.ToLower().Replace(" ", "_");
            Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToLower());
            Image = Path.Combine(ImageFolderPath, Id + ".png");
            Description = description;
            Rarity = rarity;
            IsUnique = isUnique;
            ObtainableInRegularCases = isAvailableInBeforeStageChoosingPhase;
        }
        public abstract string TextInfo();

    }
}
