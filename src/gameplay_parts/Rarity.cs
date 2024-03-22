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
        public static string ToRadialGradient(this Rarity rarity) => rarity switch
        {
            Rarity.Common => "radial-gradient(circle, #c8d8f4, #c6d7f6, #c3d7f8, #c1d6fa, #bed5fc, #bcd4fd, #b9d3fe, #b7d2ff, #b4d0ff, #b1ceff, #aeccff, #abcaff)",
            Rarity.Rare => "radial-gradient(circle, #94f887, #91f784, #8ef781, #8bf67d, #88f57a, #84f476, #81f272, #7df16e, #78ef68, #72ec63, #6dea5d, #67e757)",
            Rarity.Epic => "radial-gradient(circle, #d6aafb, #d2a1fb, #ce98fb, #c98ffb, #c586fb, #c17dfb, #bc73fc, #b869fc, #b25dfd, #ac4ffe, #a641fe, #9f30ff)",
            Rarity.Legendary => "radial-gradient(circle, #ffe56c, #fee262, #fcdf57, #fbdb4b, #fad83f, #fad637, #fad32f, #fad126, #fbce21, #fdcc1c, #fec917, #ffc611)",
            Rarity.Mythic => "radial-gradient(circle, #ff6363, #ff5e5e, #ff5a5a, #ff5555, #ff5050, #fe4a4d, #fe4449, #fd3e46, #fc3544, #fb2b42, #f91d40, #f8053e)",
            _ => "#FFFFFF",
        };
    }
}
