using SaYSpin.src.gameplay_parts;

namespace SaYSpin.src.inventory_items.relics.relic_effects
{
    public class AfterStageCompletedRelicEffect : BaseRelicEffect
    {
        public delegate void AfterStageCompletedAction(int stageNumberCompleted, GameFlowController gameFlowController);
        public AfterStageCompletedRelicEffect(string description, AfterStageCompletedAction action) : base(description)
        {
            PerformAfterStageAction = action;
        }
        public AfterStageCompletedAction PerformAfterStageAction { get; init; }
    }
}
