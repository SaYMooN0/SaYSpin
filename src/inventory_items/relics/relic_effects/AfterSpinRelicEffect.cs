using SaYSpin.src.gameplay_parts;

namespace SaYSpin.src.inventory_items.relics.relic_effects
{
    public class AfterSpinRelicEffect : BaseRelicEffect
    {
        public delegate void AfterSpinAction(GameFlowController gameFlowController);
        public AfterSpinRelicEffect(string description, AfterSpinAction action) : base(description)
        {
            PerformAfterSpinAction = action;
        }
        public AfterSpinAction PerformAfterSpinAction { get; init; }
    }
}
