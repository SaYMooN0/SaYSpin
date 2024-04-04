using SaYSpin.src.gameplay_parts;
using SaYSpin.src.inventory_items;
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
        public static List<BaseInventoryItem> GatherAllAfterStageRewards(this GameFlowController game, int currentStage) =>
            game.Inventory.Relics
                .SelectMany(r => r.Effects.OfType<AfterStageRewardRelicEffect>())
                .SelectMany(effect => effect.AfterStageReward(currentStage, game))
                .Where(reward => reward is not null)
                .ToList();
        public static void HandleAfterSpinRelicEffects(this GameFlowController game)
        {
            foreach (Relic r in game.Inventory.Relics)
            {
                foreach (AfterSpinRelicEffect rEffect in r.Effects.OfType<AfterSpinRelicEffect>())
                {
                    rEffect.PerformAfterSpinAction(game);
                }
            }
        }
        public static void HandleTileItemsWithAbsorbingEffects(this GameFlowController game)
        {
            for (int i = 0; i < game.SlotMachine.TileItems.GetLength(0); i++)
            {
                for (int j = 0; j < game.SlotMachine.TileItems.GetLength(1); j++)
                {
                    var item = game.SlotMachine.TileItems[i, j];
                    if (item is null) continue;

                    var absorbingEffects = item.Effects.OfType<AbsorbingTileItemEffect>();
                    if (!absorbingEffects.Any()) continue;

                    (int, int)[] adjacentPositions = [
                        (i - 1, j),
                        (i + 1, j),
                        (i, j - 1),
                        (i, j + 1),
                        (i - 1, j - 1), 
                        (i - 1, j + 1),
                        (i + 1, j - 1),
                        (i + 1, j + 1)
                    ];

                    foreach (var effect in absorbingEffects)
                    {
                        foreach (var (adjI, adjJ) in adjacentPositions)
                        {
                            if (adjI >= 0 && adjI < game.SlotMachine.TileItems.GetLength(0) &&
                                adjJ >= 0 && adjJ < game.SlotMachine.TileItems.GetLength(1))
                            {
                                var adjItem = game.SlotMachine.TileItems[adjI, adjJ];

                                if (adjItem != null && effect.AbsorbingCondition(adjItem))
                                {
                      
                                    effect.ExecuteOnAbsorbAction(game);

                                    game.DestroyTileItem(adjItem, adjI, adjJ);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static TileItem TileItemWithId(this GameFlowController game, string id)
        {
            var item = game.TileItems.FirstOrDefault(item => item?.Id == id);
            if (item is null)
                throw new InvalidOperationException($"TileItem with ID '{id}' not found.");
            return item;
        }
        public static Relic RelicWithId(this GameFlowController game, string id)
        {
            var relic = game.Relics.FirstOrDefault(relic => relic?.Id == id);
            if (relic is null)
                throw new InvalidOperationException($"Relic with ID '{id}' not found.");
            return relic;
        }
        public static bool CoinsEnoughToCompleteTheStage(this GameFlowController game) =>
            game.CoinsCount >= game.CoinsNeededToCompleteTheStage;
    }
}
