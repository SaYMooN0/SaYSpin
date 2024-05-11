using SaYSpin.src.gameplay_parts.game_flow_controller;
using SaYSpin.src.inventory_items;
using SaYSpin.src.inventory_items.relics;
using SaYSpin.src.inventory_items.relics.relic_effects;
using SaYSpin.src.inventory_items.tile_items;

namespace SaYSpin.src.extension_classes
{
    public static class GameFlowExtensions
    {
        public static List<BaseInventoryItem> GatherAllAfterStageRewards(this GameFlowController game) =>
            game.Inventory.Relics
                .SelectMany(r => r.Effects.OfType<AfterStageRewardRelicEffect>())
                .SelectMany(effect => effect.AfterStageReward(game.CurrentStage, game))
                .Where(reward => reward is not null)
                .ToList();

        public static void UpdateStatsIfNeeded(this GameFlowController game)
        {
            if (!game.StatsTracker.Changed)
                return;

            var staticEffects = game.Inventory.Relics
                .SelectMany(r => r.Effects.OfType<GameStatRelicEffect>());

            var dynamicEffects = game.Inventory.Relics
                .SelectMany(r => r.Effects.OfType<NonConstantGameStatRelicEffect>())
                .Select(de => de.ConvertToStaticEffect(game))
                .Where(e => e != null);

            var combinedEffects = staticEffects.Concat(dynamicEffects);

            game.StatsTracker.Update(combinedEffects);
        }
        public static TileItem? TileItemWithId(this GameFlowController game, string id) =>
            game.AllTileItemsCollection.FirstOrDefault(item => item?.Id == id);
        public static Relic? RelicWithId(this GameFlowController game, string id) =>
            game.AllRelicsCollection.FirstOrDefault(relic => relic?.Id == id);
        public static bool CoinsEnoughToCompleteTheStage(this GameFlowController game) =>
            game.CoinsCount >= game.CoinsNeededToCompleteTheStage;
    }
}
