using SaYSpin.src.gameplay_parts.game_flow_controller;

namespace SaYSpin.src.inventory_items.tile_items.tile_item_effects
{
    public class OnDestroyTileItemEffect : BaseTileItemEffect
    {
        public OnDestroyTileItemEffect(string description, Action<GameFlowController> onDestroyAction) : base(description)
        {
            PerformOnDestroyAction = onDestroyAction;
        }
        public Action<GameFlowController> PerformOnDestroyAction { get; init; }
    }
}
