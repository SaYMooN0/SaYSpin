using SaYSpin.src.gameplay_parts;

namespace SaYSpin.src.inventory_items.tile_items.tile_item_effects
{
    public class OnDestroyTileItemEffect : BaseTileItemEffect
    {
        public delegate void OnDestroyAction(GameFlowController gameFlowController);
        public OnDestroyTileItemEffect(string description, OnDestroyAction action) : base(description)
        {
            PerformOnDestroyAction = action;
        }
        public OnDestroyAction PerformOnDestroyAction { get; init; }
    }
}
