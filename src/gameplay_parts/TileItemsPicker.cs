using SaYSpin.src.inventory_items.tile_items;

namespace SaYSpin.src.gameplay_parts
{
    public class TileItemsPicker
    {
        private readonly int _itemsNeeded;
        private List<TileItem> PickedItems { get; set; }

        public TileItemsPicker(int itemsNeeded)
        {
            _itemsNeeded = itemsNeeded;
            PickedItems = new List<TileItem>();
        }

        public void PickItemsFrom(List<TileItem> itemsToPickFrom)
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

        public List<TileItem> GetRandomizedItems()
        {
            Random rand = new Random();
            return PickedItems.OrderBy(x => rand.Next()).ToList();
        }

        private IEnumerable<TileItem> RandomSelection(List<TileItem> source, int count, Random rand) =>
            source.OrderBy(x => rand.Next()).Take(count);
    }
}
