using SaYSpin.src.gameplay_parts.game_flow_controller;

namespace SaYSpin.src.inventory_items.tile_items.tile_item_effects
{
    internal class TransformationTileItemEffect : BaseTileItemEffect
    {
        public TransformationTileItemEffect(string description, Func<GameFlowController, bool> transformationCondition, TileItem tileItemToTransformInto)
            : base(description)
        {
            TransformationCondition = transformationCondition;
            TileItemToTransformInto = tileItemToTransformInto;
        }

        public TileItem TileItemToTransformInto { get; init; }
        public Func<GameFlowController, bool> TransformationCondition { get; init; }

    }
}
