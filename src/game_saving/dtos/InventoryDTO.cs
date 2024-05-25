using SaYSpin.src.gameplay_parts.inventory_related;
using SaYSpin.src.gameplay_parts.inventory_related.tokens;
using SaYSpin.src.inventory_items.relics;
using SaYSpin.src.inventory_items.tile_items;

namespace SaYSpin.src.game_saving.dtos
{
    public class InventoryDTO
    {
        public int DiamondsCount { get; set; }
        public List<InventoryItemDTO> TileItems { get; set; } = new();
        public List<InventoryItemDTO> Relics { get; set; } = new();
        public int ShopRefreshTokensCount { get; set; }
        public int TileItemRemovalTokensCount { get; set; }
        public int NewStageItemsRefreshTokensCount { get; set; }
        public static InventoryDTO FromInventory(Inventory inventory)
        {
            return new InventoryDTO
            {
                TileItems = inventory.TileItems
                    .Select(InventoryItemDTO.FromItem)
                    .ToList(),
                Relics = inventory.Relics
                    .Select(InventoryItemDTO.FromItem)
                    .ToList(),
                NewStageItemsRefreshTokensCount = inventory.Tokens.Count(TokenType.NewStageItemsRefresh),
                ShopRefreshTokensCount = inventory.Tokens.Count(TokenType.ShopRefresh),
                TileItemRemovalTokensCount = inventory.Tokens.Count(TokenType.InventoryItemRemoval),
                DiamondsCount = inventory.DiamondsCount
            };
        }

        internal Inventory ToInventory(
          IDictionary<string, Func<TileItem>> tileItemConstructors,
          IDictionary<string, Func<Relic>> relicConstructors)
        {
            var tokens = new TokensCollection(
                newStageItemsRefreshTokensCount: NewStageItemsRefreshTokensCount,
                shopRefreshTokensCount: ShopRefreshTokensCount,
                tileItemRemovalTokensCount: TileItemRemovalTokensCount);

            List<TileItem> tileItems = TileItems.Select(dto => dto.ToItem(tileItemConstructors)).ToList();
            List<Relic> relics = Relics.Select(dto => dto.ToItem(relicConstructors)).ToList();

            return new Inventory(tileItems, relics, tokens, DiamondsCount);
        }
    }
}
