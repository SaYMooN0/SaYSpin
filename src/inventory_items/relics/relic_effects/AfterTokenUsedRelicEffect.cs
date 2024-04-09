using SaYSpin.src.gameplay_parts;

namespace SaYSpin.src.inventory_items.relics.relic_effects
{
    internal class AfterTokenUsedRelicEffect : BaseRelicEffect
    {
        public delegate void AfterTokenUsedAction(GameFlowController gameFlowController);
        public AfterTokenUsedRelicEffect(string description, AfterTokenUsedAction action) : base(description)
        {
            PerformAfterTokenUsedAction = action;
        }
        public AfterTokenUsedAction PerformAfterTokenUsedAction { get; init; }
   
    }
}
