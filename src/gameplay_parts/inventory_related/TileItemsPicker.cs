using SaYSpin.src.abstract_classes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SaYSpin.src.gameplay_parts.inventory_related
{
    public class TileItemsPicker
    {
        private readonly int _itemsNeeded;
        private List<BaseTileItem> PickedItems { get; set; }

        public TileItemsPicker(int itemsNeeded)
        {
            _itemsNeeded = itemsNeeded;
            PickedItems = new List<BaseTileItem>();
        }

        public void PickItemsFrom(List<BaseTileItem> itemsToPickFrom)
        {
            Random rand = new Random();
            PickedItems.Clear();

            if (itemsToPickFrom.Count <= _itemsNeeded)
            {
                PickedItems.AddRange(itemsToPickFrom);
                if (PickedItems.Count >= 8)
                {
                    int indexToRemove = rand.Next(PickedItems.Count);
                    PickedItems.RemoveAt(indexToRemove);
                }
            }
            else if (itemsToPickFrom.Count <= _itemsNeeded + 2)
            {
                int itemsToPick = rand.Next(_itemsNeeded - 1, _itemsNeeded + 1);
                PickedItems.AddRange(RandomSelection(itemsToPickFrom, itemsToPick, rand));
            }
            else
            {
                PickedItems.AddRange(RandomSelection(itemsToPickFrom, _itemsNeeded, rand));
            }
        }

        public List<BaseTileItem> GetRandomizedItems()
        {
            Random rand = new Random();
            return PickedItems.OrderBy(x => rand.Next()).ToList();
        }

        private IEnumerable<BaseTileItem> RandomSelection(List<BaseTileItem> source, int count, Random rand) =>
            source.OrderBy(x => rand.Next()).Take(count);
    }
}
