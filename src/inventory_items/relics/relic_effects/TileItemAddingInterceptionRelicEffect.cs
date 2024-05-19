using SaYSpin.src.gameplay_parts.game_flow_controller;
using SaYSpin.src.inventory_items.tile_items;

namespace SaYSpin.src.inventory_items.relics.relic_effects
{
    internal class TileItemAddingInterceptionRelicEffect : BaseRelicEffect
    {
        public delegate TileItem? TileItemAddingInterceptionDelegate(GameFlowController game, TileItem initialTileItemToAdd);
        public TileItemAddingInterceptionRelicEffect(string description, TileItemAddingInterceptionDelegate interceptionFunc, Func<TileItem, bool> interceptionCondition)
           : base(description)
        {
            InterceptionCondition = interceptionCondition;
            InterceptionFunc = interceptionFunc;
        }
        public Func<TileItem, bool> InterceptionCondition { get; init; }
        public TileItemAddingInterceptionDelegate InterceptionFunc { get; init; }
    }
}
