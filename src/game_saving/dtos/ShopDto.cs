namespace SaYSpin.src.game_saving.dtos
{
    public record class ItemForSaleDTO(string Name, int Price, bool isLocked = false);
    public class ShopDTO
    {
        public List<ItemForSaleDTO> TileItemsForSale { get; private set; } = [];
        public List<ItemForSaleDTO> RelicsForSale { get; private set; } = [];
        public string[] PossibleSpecialMerchantsNames { get; init; }
    }
}
