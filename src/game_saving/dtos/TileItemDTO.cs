using SaYSpin.src.inventory_items.tile_items;

namespace SaYSpin.src.game_saving.dtos
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    internal class TileItemDTO
    {
        public string Name { get; init; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Counter { get; init; }

        public static TileItemDTO FromTileItem(TileItem ti) =>
            new TileItemDTO
            {
                Name = ti.Name,
                Counter = (ti is TileItemWithCounter tiWithCounter) ? tiWithCounter.Counter : null
            };

        public TileItem ToTileItem(IDictionary<string, Func<TileItem>> tileItemConstructors)
        {
            if (!tileItemConstructors.ContainsKey(Name))
                throw new ArgumentException($"Constructor for TileItem with name '{Name}' not found.");

            TileItem ti = tileItemConstructors[Name]();
            if (ti is TileItemWithCounter tiWithCounter)
            {
                if (Counter is int counter)
                    return TileItemWithCounter.WithCounterValue(tiWithCounter, counter);
            }
            return ti;
        }
    }

}
