using SaYSpin.src.abstract_classes;

namespace SaYSpin.src.singletons
{
    public class ShowItemInfoDialogService
    {
        public event Func<BaseInventoryItem, Task> OnShow;
        public async Task ShowDialog(BaseInventoryItem item)
        {
            try
            {
                if (OnShow != null)
                {
                    await OnShow(item);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }

        }
    }
}
