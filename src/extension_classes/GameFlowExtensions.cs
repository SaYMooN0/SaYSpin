using SaYSpin.src.gameplay_parts;
using SaYSpin.src.inventory_items.relics;
using SaYSpin.src.inventory_items.relics.relic_effects;
using SaYSpin.src.inventory_items.tile_items;
using SaYSpin.src.inventory_items.tile_items.tile_item_effects;

namespace SaYSpin.src.extension_classes
{
    public static class GameFlowExtensions
    {

        public static List<GameStarterKit> GenerateStarterKits(this GameFlowController game)
        {
            List<GameStarterKit> kits = new();
            var commonItems = game.TileItems.Where(i => i.Rarity == Rarity.Common).OrderBy(x => Guid.NewGuid()).ToList();
            var rareItems = game.TileItems.Where(i => i.Rarity == Rarity.Rare).OrderBy(x => Guid.NewGuid()).ToArray();

            var relics = game.Relics.Where(r => r.Rarity <= Rarity.Rare).OrderBy(x => Guid.NewGuid()).ToArray();

            var totalKits = 4;
            int commonItemsPerKit = 3;

            for (int i = 0; i < totalKits; i++)
            {
                var itemsForKit = commonItems.Skip(i * commonItemsPerKit).Take(commonItemsPerKit).ToList();
                itemsForKit.Add(rareItems[i % rareItems.Length]);

                kits.Add(new GameStarterKit(itemsForKit, [relics[i % relics.Length]],
                    GameStarterKit.RandomTokensCollection(game.Difficulty.StartingTokensCount), game.Difficulty.StartingDiamondsCount));
            }

            return kits;
        }
        public static int CalculateCoinsNeededForStage(this GameFlowController game, int stageToCalculateFor)
        {
            return (int)(
                Math.Pow(stageToCalculateFor * 0.9, 1.8)
                * (game.Difficulty.NeededCoinsMultiplier + 1) * 3.2
                * game.CoinsNeededToCompleteTheStageCoefficient()
            ) + 10;
        }
        public static TileItem[] GenerateTileItemsForNewStageChoosing(this GameFlowController game)
        {

            return game.TileItems.OrderBy(x => Guid.NewGuid()).Take(5).ToArray();
            //will be changed
        }
        public static Relic[] GenerateRelicsForNewStageChoosing(this GameFlowController game)
        {
            return game.Relics.OrderBy(x => Guid.NewGuid()).Take(5).ToArray();
            //will be changed
        }
        public static void ExecuteAfterStageRelicEffects(this GameFlowController game, int stageNumberCompleted)
        {
            foreach (Relic r in game.Inventory.Relics)
            {
                foreach (AfterStageCompletedRelicEffect rEffect in r.Effects.OfType<AfterStageCompletedRelicEffect>())
                {
                    rEffect.PerformAfterStageAction(stageNumberCompleted, game);
                }
            }
        }
        public static void ExecuteAfterSpinRelicEffects(this GameFlowController game)
        {
            foreach (Relic r in game.Inventory.Relics)
            {
                foreach (AfterSpinRelicEffect rEffect in r.Effects.OfType<AfterSpinRelicEffect>())
                {
                    rEffect.PerformAfterSpinAction(game);
                }
            }
        }
    }
}
