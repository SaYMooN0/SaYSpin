using SaYSpin.src.gameplay_parts;

namespace SaYSpin.src.inventory_items.tile_items.tile_item_effects
{
    internal class AbsorbingTileItemEffect : BaseTileItemEffect
    {
        public AbsorbingTileItemEffect(string description, Func<TileItem, bool> absorbingCondition, OnAbsorbActionDelegate onAbsorbAction) : base(description)
        {
            AbsorbingCondition = absorbingCondition;
            ExecuteOnAbsorbAction = onAbsorbAction;
        }
        public OnAbsorbActionDelegate ExecuteOnAbsorbAction { get; init; }
        public Func<TileItem, bool> AbsorbingCondition{ get; init; }
        public delegate void OnAbsorbActionDelegate(GameFlowController gameController);
    }
}
