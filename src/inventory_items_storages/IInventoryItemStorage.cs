namespace SaYSpin.src.inventory_items_storages
{
    internal interface IInventoryItemStorage<TItem>
    {
        void Update();
        HashSet<string> InitAvailable();
        void SaveToFile();
        Dictionary<string, Func<TItem>> GetAvailableItems();
    }
}
