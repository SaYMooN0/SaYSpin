namespace SaYSpin.src.gameplay_parts
{
    public enum Rarity
    {
        Common,
        Rare,
        Epic,
        Legendary,
        Mythic

    }
    public static class RarityExtensions
    {

        public static string ToHexColor(this Rarity rarity) => rarity switch
        {
            Rarity.Common => "#B1C8F0",
            Rarity.Rare => "#4FE93C",
            Rarity.Epic => "#a63fff",
            Rarity.Legendary => "#ffc611",
            Rarity.Mythic => "#ff033e",
            _ => "#FFFFFF",
        };
    }
}
