using SaYSpin.src.enums;
using SaYSpin.src.gameplay_parts;
using SaYSpin.src.gameplay_parts.inventory_related.tokens;
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
            var rareItems = game.TileItems.Where(i => i.Rarity == Rarity.Rare).OrderBy(x => Guid.NewGuid()).ToList();
            var relics = game.Relics.Where(r => r.Rarity <= Rarity.Rare).OrderBy(x => Guid.NewGuid()).ToList();

            int totalKits = 4;
            int commonItemsPerKit = game.Difficulty.StartingTileItemsCount;
            int relicsPerKit = game.Difficulty.StartingRelicsCount;

            for (int i = 0; i < totalKits; i++)
            {
                List<TileItem> itemsForKit = new();
                for (int j = 0; j < commonItemsPerKit; j++)
                {
                    itemsForKit.Add(commonItems[(i * commonItemsPerKit + j) % commonItems.Count]);
                }
                itemsForKit.Add(rareItems[i % rareItems.Count]);

                List<Relic> relicsForKit = new();
                for (int j = 0; j < relicsPerKit; j++)
                {
                    relicsForKit.Add(relics[(i * relicsPerKit + j) % relics.Count]);
                }

                kits.Add(new GameStarterKit(itemsForKit, relicsForKit,
                    GameStarterKit.RandomTokensCollection(game.Difficulty.StartingTokensCount), game.Difficulty.StartingDiamondsCount));
            }

            return kits;
        }

        public static int CalculateCoinsNeededForStage(this GameFlowController game)
        {
            return (int)(
                Math.Pow(game.CurrentStage * 1.65 + 2, 1.85)
                * (game.Difficulty.NeededCoinsMultiplier + 0.2)
                * 2
                ) - 10;
        }
        public static TileItem[] GenerateTileItemsForNewStageChoosing(this GameFlowController game)
        {

            return game.TileItems.OrderBy(x => Guid.NewGuid()).Take(game.StatsTracker.NewStageTileItemsForChoiceCount).ToArray();
            //will be changed
        }
        public static Relic[] GenerateRelicsForNewStageChoosing(this GameFlowController game)
        {
            return game.Relics.OrderBy(x => Guid.NewGuid()).Take(game.StatsTracker.NewStageRelicsForChoiceCount).ToArray();
            //will be changed
        }
        public static List<BaseInventoryItem> GatherAllAfterStageRewards(this GameFlowController game) =>
            game.Inventory.Relics
                .SelectMany(r => r.Effects.OfType<AfterStageRewardRelicEffect>())
                .SelectMany(effect => effect.AfterStageReward(game.CurrentStage, game))
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
        public static void HandleTileItemsWithAreaScanningEffects(this GameFlowController game)
        {

            for (int i = 0; i < game.SlotMachine.TileItems.GetLength(0); i++)
            {
                for (int j = 0; j < game.SlotMachine.TileItems.GetLength(1); j++)
                {
                    var tI = game.SlotMachine.TileItems[i, j];
                    if (tI is null) continue;
                    foreach (var effect in tI.Effects.OfType<AreaScanningTileItemEffect>())
                    {
                        List<TileItem> tileItemsInArea = new();
                        switch (effect.Area)
                        {

                            case SlotMachineArea.Adjacent:
                                {
                                    var offsets = new[] { (-1, 0), (1, 0), (0, -1), (0, 1), (1, 1), (1, -1), (-1, 1), (-1, -1) };
                                    foreach (var (di, dj) in offsets)
                                    {
                                        int newI = i + di, newJ = j + dj;
                                        if (newI >= 0 && newI < game.SlotMachine.TileItems.GetLength(0) && newJ >= 0 && newJ < game.SlotMachine.TileItems.GetLength(1))
                                        {
                                            var tileItem = game.SlotMachine.TileItems[newI, newJ];
                                            if (effect.Condition(tileItem))
                                                tileItemsInArea.Add(tileItem);
                                        }
                                    }
                                }
                                break;
                            default:
                                throw new NotImplementedException($"EffectApplicationArea {effect.Area} in method `HandleTileItemsWithAreaScanningEffect` is not implemented.");
                        }
                        effect.PerformOnScannedAction(game, tileItemsInArea);
                    }

                }
            }
        }

        public static void HandleTransformationEffects(this GameFlowController game)
        {
            List<Action> replacements = new();
            foreach (TileItem ti in game.Inventory.TileItems)
            {
                foreach (var effect in ti.Effects.OfType<TransformationTileItemEffect>())
                {
                    if (effect.TransformationCondition(game))
                    {
                        replacements.Add(() => game.ReplaceTileItem(ti, effect.TileItemToTransformInto));
                        break;
                    }
                }
            }
            foreach (Action replace in replacements)
                replace();
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

                                    effect.ExecuteOnAbsorbAction(game, adjItem);

                                    game.DestroyTileItem(adjItem, adjI, adjJ);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static IEnumerable<CoinsCalculationRelicEffect> GatherCoinsCalculationRelicEffects(this GameFlowController game)
        {

            IEnumerable<CoinsCalculationRelicEffect> effects = game.Inventory.Relics
               .SelectMany(relic => relic.Effects.OfType<CoinsCalculationRelicEffect>());

            IEnumerable<CoinsCalculationRelicEffect> nonConstantEffects = game.Inventory.Relics
               .SelectMany(relic => relic.Effects.OfType<NonConstantCalculationRelicEffect>())
               .Select(eff => eff.GetCalculationEffect(game));

            return effects.Concat(nonConstantEffects);
        }

        public static void UpdateStatsIfNeeded(this GameFlowController game)
        {
            if (!game.StatsTracker.Changed)
                return;
            var effects = game.Inventory.Relics
                    .SelectMany(r => r.Effects.OfType<GameStatRelicEffect>());
            game.StatsTracker.Update(effects);
        }

        public static void TriggerOnNewStageChoosingSkippedEffects(this GameFlowController game)
        {
            var effects = game.Inventory.Relics.SelectMany(r => r.Effects).OfType<OnNewStageChoosingSkippedRelicEffect>();
            foreach (var effect in effects)
            {
                effect.PerformOnNewStageChoosingSkippedAction(game);
            }
        }
        public static void TriggerOnTokenUsedEffects(this GameFlowController game, TokenType token)
        {
            var effects = game.Inventory.Relics.SelectMany(r => r.Effects).OfType<OnTokenUsedRelicEffect>();
            foreach (var effect in effects)
            {
                effect.PerformOnTokenUsedAction(game, token);
            }
        }

        public static TileItem? TileItemWithId(this GameFlowController game, string id) =>
            game.TileItems.FirstOrDefault(item => item?.Id == id);
        public static Relic? RelicWithId(this GameFlowController game, string id) =>
            game.Relics.FirstOrDefault(relic => relic?.Id == id);
        public static bool CoinsEnoughToCompleteTheStage(this GameFlowController game) =>
            game.CoinsCount >= game.CoinsNeededToCompleteTheStage;
    }
}
