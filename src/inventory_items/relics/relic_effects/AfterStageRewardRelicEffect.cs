using SaYSpin.src.gameplay_parts.game_flow_controller;

namespace SaYSpin.src.inventory_items.relics.relic_effects
{
    public class AfterStageRewardRelicEffect : BaseRelicEffect
    {
        public delegate List<BaseInventoryItem?> AfterStageCompletedReward(int stageNumberCompleted, GameFlowController gameFlowController);
        public AfterStageRewardRelicEffect(string description, AfterStageCompletedReward reward) : base(description)
        {
            AfterStageReward = reward;
        }
        public AfterStageCompletedReward AfterStageReward { get; init; }
    }
}
