using SaYSpin.src.inventory_items;
using System.Text.Json.Serialization;
using SaYSpin.src.inventory_items.relics;
using SaYSpin.src.inventory_items.tile_items;

namespace SaYSpin.src.game_saving.dtos
{
    public record class InventoryItemDTO
    {
        public string Name { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Counter { get; init; }

        public static InventoryItemDTO FromItem(BaseInventoryItem item) =>
            new InventoryItemDTO
            {
                Name = item.Name,
                Counter = item is IWithCounter counterItem ? counterItem.Counter : null
            };

        public T ToItem<T>(IDictionary<string, Func<T>> itemConstructors) where T : BaseInventoryItem
        {
            if (!itemConstructors.ContainsKey(Name))
                throw new ArgumentException($"Constructor for item with name '{Name}' not found.");

            T item = itemConstructors[Name]();
            if (item is IWithCounter)
            {
                if (Counter is int counter)
                {
                    item = item switch
                    {
                        TileItemWithCounter tileItem => TileItemWithCounter.WithCounterValue(tileItem, counter) as T,
                        RelicWithCounter relicItem => RelicWithCounter.WithCounterValue(relicItem, counter) as T,
                        _ => item
                    };
                }
            }
            return item;
        }
    }
}


