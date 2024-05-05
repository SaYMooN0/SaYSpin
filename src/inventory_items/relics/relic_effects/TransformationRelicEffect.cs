using SaYSpin.src.gameplay_parts.game_flow_controller;
namespace SaYSpin.src.inventory_items.relics.relic_effects
{
    internal class TransformationRelicEffect : BaseRelicEffect
    {
        public TransformationRelicEffect(string description, Func<GameFlowController, bool> transformationCondition, Relic relicToTransformInto)
            : base(description)
        {
            TransformationCondition = transformationCondition;
            RelicToTransformInto = relicToTransformInto;
        }

        public Relic RelicToTransformInto { get; init; }
        public Func<GameFlowController, bool> TransformationCondition { get; init; }
    }
}
