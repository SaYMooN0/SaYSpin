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
        public static int ItemPrice(this Rarity rarity) => rarity switch
        {
            Rarity.Common => 3,
            Rarity.Rare => 5,
            Rarity.Epic => 7,
            Rarity.Legendary => 9,
            Rarity.Mythic => 12,
            _ => 0
        };
    }
}
