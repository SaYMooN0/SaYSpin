using SaYSpin.src.gameplay_parts;

namespace SaYSpin.src.inventory_items.tile_items.tile_item_effects
{
    public class OnDestroyTileItemEffect : BaseTileItemEffect
    {
        public OnDestroyTileItemEffect(string description,TileItem sourceTileItem, Action<GameFlowController> onDestroyAction) : base(description, sourceTileItem)
        {
            PerformOnDestroyAction = onDestroyAction;
        }
        public Action<GameFlowController> PerformOnDestroyAction { get; init; }
    }
}
