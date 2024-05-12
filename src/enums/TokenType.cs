namespace SaYSpin.src.gameplay_parts.inventory_related.tokens
{
    public enum TokenType
    {
        ShopRefresh,
        InventoryItemRemoval,
        NewStageItemsRefresh
    }
    public static class TokenTypeExtensions
    {
        public static string GetImage(this TokenType tokenType)
        {
            return tokenType switch
            {
                TokenType.ShopRefresh => "shop_refresh.png",
                TokenType.InventoryItemRemoval => "inventory_item_removal.png",
                TokenType.NewStageItemsRefresh => "new_stage_items_refresh.png",
                _ => throw new ArgumentException("Unsupported token type"),
            };
        }
        public static string ImageFullPath(this TokenType tokenType) =>
            "resources/images/inventory/tokens/" + tokenType.GetImage();
    }
}
