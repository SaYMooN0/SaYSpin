using SaYSpin.src.gameplay_parts.game_flow_controller;

namespace SaYSpin.src.inventory_items.relics.relic_effects
{
    internal class OnStageStartedRelicEffect : BaseRelicEffect
    {
        public delegate void OnStageStartedAction(GameFlowController game);
        public OnStageStartedRelicEffect(string description, OnStageStartedAction action) : base(description)
        {
            PerformOnStageStartedAction = action;
        }
        public OnStageStartedAction PerformOnStageStartedAction { get; init; }
    }
}
