using SaYSpin.src.gameplay_parts.game_flow_controller;
using SaYSpin.src.gameplay_parts.inventory_related.tokens;

namespace SaYSpin.src.inventory_items.relics.relic_effects
{
    internal class OnTokenUsedRelicEffect : BaseRelicEffect
    {
        public delegate void OnTokenUsedAction(GameFlowController gameFlowController, TokenType token);
        public OnTokenUsedRelicEffect(string description, OnTokenUsedAction action) : base(description)
        {
            PerformOnTokenUsedAction = action;
        }
        public OnTokenUsedAction PerformOnTokenUsedAction { get; init; }

    }
}
