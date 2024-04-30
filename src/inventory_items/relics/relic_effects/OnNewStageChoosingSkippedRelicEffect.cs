using SaYSpin.src.gameplay_parts.game_flow_controller;

namespace SaYSpin.src.inventory_items.relics.relic_effects
{
    internal class OnNewStageChoosingSkippedRelicEffect : BaseRelicEffect
    {
        public delegate void OnNewStageChoosingSkippedAction(GameFlowController game);
        public OnNewStageChoosingSkippedRelicEffect(string description, OnNewStageChoosingSkippedAction action) : base(description)
        {
            PerformOnNewStageChoosingSkippedAction = action;
        }
        public OnNewStageChoosingSkippedAction PerformOnNewStageChoosingSkippedAction { get; init; }
    }
}
