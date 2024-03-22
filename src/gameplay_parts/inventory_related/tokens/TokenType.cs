namespace SaYSpin.src.gameplay_parts.inventory_related.tokens
{
    public enum TokenType
    {
        AddSpin,
        FreeShopRefresh,
        TileItemRemoval,
        NewStageRelicChoiceRefresh,
        NewStageItemChoiceRefresh
    }
    public static class TokenTypeExtensions
    {
        public static string GetImage(this TokenType tokenType)
        {
            return tokenType switch
            {
                TokenType.AddSpin => "add_spin_token.png",
                TokenType.FreeShopRefresh => "free_shop_refresh.png",
                TokenType.TileItemRemoval => "tile_item_removal.png",
                TokenType.NewStageRelicChoiceRefresh => "new_stage_relic_choice_refresh.png",
                TokenType.NewStageItemChoiceRefresh => "new_stage_item_choice_refresh.png",
                _ => throw new ArgumentException("Unsupported token type"),
            };
        }
        public static string ImageFullPath(this TokenType tokenType) =>
            "resources/images/inventory/tokens/"+tokenType.GetImage();
    }
}
