using SaYSpin.src.gameplay_parts.inventory_related;
using SaYSpin.src.gameplay_parts.inventory_related.tokens;
using SaYSpin.src.inventory_items.relics;
using SaYSpin.src.inventory_items.tile_items;

namespace SaYSpin.src.game_saving.dtos
{
    public class InventoryDTO
    {
        public int DiamondsCount { get; set; }
        internal List<TileItemDTO> TileItems { get; set; }
        internal List<RelicDTO> Relics { get; set; }
        internal int ShopRefreshTokensCount { get; set; }
        internal int TileItemRemovalTokensCount { get; set; }
        internal int NewStageItemsRefreshTokensCount { get; set; }
        internal static InventoryDTO FromInventory(Inventory inventory)
        {
            return new InventoryDTO
            {
                TileItems = inventory.TileItems
                    .Select(TileItemDTO.FromTileItem)
                    .ToList(),
                Relics = inventory.Relics
                    .Select(RelicDTO.FromRelic)
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

            var tileItems = TileItems.Select(dto => dto.ToTileItem(tileItemConstructors)).ToList();
            var relics = Relics.Select(dto => dto.ToRelic(relicConstructors)).ToList();

            return new Inventory(tileItems, relics, tokens, DiamondsCount);
        }
    }
}
