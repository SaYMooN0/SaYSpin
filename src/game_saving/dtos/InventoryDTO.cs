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

    }
}
