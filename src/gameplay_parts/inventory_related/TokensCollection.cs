namespace SaYSpin.src.gameplay_parts.inventory_related
{
    public class TokensCollection
    {
        public TokensCollection(
            int addSpinTokensCount = 0,
            int freeShopRefreshTokensCount = 0,
            int tileItemRemovalTokensCount = 0,
            int newStageRelicChoiceRefreshTokensCount = 0,
            int newStageItemChoiceRefreshTokensCount = 0)
        {
            AddSpinTokensCount = addSpinTokensCount;
            FreeShopRefreshTokensCount = freeShopRefreshTokensCount;
            TileItemRemovalTokensCount = tileItemRemovalTokensCount;
            NewStageRelicChoiceRefreshTokensCount = newStageRelicChoiceRefreshTokensCount;
            NewStageItemChoiceRefreshTokensCount = newStageItemChoiceRefreshTokensCount;
        }

        public int AddSpinTokensCount { get; private set; }
        public int FreeShopRefreshTokensCount { get; private set; }
        public int TileItemRemovalTokensCount { get; private set; }
        public int NewStageRelicChoiceRefreshTokensCount { get; private set; }
        public int NewStageItemChoiceRefreshTokensCount { get; private set; }

    }
}
