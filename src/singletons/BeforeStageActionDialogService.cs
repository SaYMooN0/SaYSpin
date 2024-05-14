using SaYSpin.src.gameplay_parts.run_progress;

namespace SaYSpin.src.singletons
{
    public class BeforeStageActionDialogService
    {
        public Func<BeforeStageActionsGroup, Task> OnShow { get; set; }
        public void Clear() => OnShow = null;
        public async Task ShowDialog(BeforeStageActionsGroup actionsGroup)
        {
            if (OnShow is not null)
            {
                await OnShow(actionsGroup);
            }

        }
    }
}
