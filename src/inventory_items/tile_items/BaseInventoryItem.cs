using SaYSpin.src.enums;
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
        public bool IsSpecial{get;init;}
        public abstract string ImageFolderPath { get; }
        internal HashSet<Markers> Markers { get; init; } = new();
        internal void AddMarker(Markers marker) =>
            Markers.Add(marker);
        internal void ClearMarkers() =>
            Markers.Clear();
        protected BaseInventoryItem(string name, string description, Rarity rarity, bool isSpecial=false, bool isUnique=false)
        {
            Id = name.ToLower().Replace(" ", "_");
            Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToLower());
            Image = Path.Combine(ImageFolderPath, Id + ".png");
            Description = description;
            Rarity = rarity;
            IsUnique = isUnique;
            IsSpecial = isSpecial;
        }
        public abstract string TextInfo();

    }
}
