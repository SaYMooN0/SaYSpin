using SaYSpin.src.inventory_items;

namespace SaYSpin.src.singletons
{
    public class ShowItemInfoDialogService
    {
        public event Func<BaseInventoryItem, Task> OnShow;
        public async Task ShowDialog(BaseInventoryItem item)
        {
            if (OnShow != null)
            {
                await OnShow(item);
            }

        }
    }
}
