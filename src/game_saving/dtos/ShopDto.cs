using SaYSpin.src.gameplay_parts.shop;
using SaYSpin.src.inventory_items;
using SaYSpin.src.inventory_items.relics;
using SaYSpin.src.inventory_items.tile_items;

namespace SaYSpin.src.game_saving.dtos
{
    public record class ItemForSaleDTO(string Name, int Price, bool IsLocked)
    {
        public static ItemForSaleDTO FromItemForSale(ItemForSale itemForSale) =>
            new(itemForSale.Item.Name, itemForSale.Price, itemForSale.IsLocked);
        public ItemForSale ToItemForSale<T>(IDictionary<string, Func<T>> itemConstructors) where T : BaseInventoryItem
        {
            if (!itemConstructors.ContainsKey(Name))
                throw new ArgumentException($"Constructor for item with name '{Name}' not found.");

            var itemForSale = new ItemForSale(itemConstructors[Name](), Price);
            
            if (IsLocked) itemForSale.Lock();
            return itemForSale;
        }
    }
    public class ShopDTO
    {
        public List<ItemForSaleDTO> TileItemsForSale { get; init; } 
        public List<ItemForSaleDTO> RelicsForSale { get; init; } 
        public string[] PossibleSpecialMerchantsNames { get; init; } = Array.Empty<string>();

        public static ShopDTO FromShopController(ShopController shopController)
        {
            return new ShopDTO
            {
                TileItemsForSale = shopController.TileItemsForSale
                    .Select(ItemForSaleDTO.FromItemForSale)
                    .ToList(),
                RelicsForSale = shopController.RelicsForSale
                    .Select(ItemForSaleDTO.FromItemForSale)
                    .ToList(),
                PossibleSpecialMerchantsNames = shopController.PossibleSpecialMerchants
                    .Select(merchant => merchant.GetType().Name)
                    .ToArray()
            };
        }

        public ShopController ToShopController(
            IDictionary<string, Func<TileItem>> tileItemConstructors,
            IDictionary<string, Func<Relic>> relicsConstructors,
            IDictionary<string, Func<ISpecialMerchant>> specialMerchantConstructors)
        {
            var tileItemsForSale = TileItemsForSale
                .Select(dto => dto.ToItemForSale(tileItemConstructors))
                .ToList();

            var relicsForSale = RelicsForSale
                .Select(dto => dto.ToItemForSale(relicsConstructors))
                .ToList();

            var possibleSpecialMerchants = PossibleSpecialMerchantsNames
                .Select(name => specialMerchantConstructors[name]())
                .ToArray();


            ShopController shopController = new(possibleSpecialMerchants);
            shopController.Update(tileItemsForSale, relicsForSale);
            return shopController;
        }
    }
}
