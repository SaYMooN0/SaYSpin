﻿
using SaYSpin.src.enums;
using SaYSpin.src.inventory_items.relics.relic_effects;

namespace SaYSpin.src.gameplay_parts
{
    public class StatsTracker
    {
        private readonly Dictionary<GameStat, double> initialValues;
        public Dictionary<GameStat, double> Values { get; private set; }
        public bool Changed { get; private set; } = true;

        public StatsTracker(
            double initLuck,
            int initNewStageTileItemsForChoiceCount,
            int initNewStageRelicsForChoiceCount,
            short initStageSpinsCount,
            double initAfterStageCoinsToDiamondsCoefficient,
            double initShopPriceCoefficient,
            int initTileItemsInShopCount,
            int initRelicsInShopCount
            )
        {
            initialValues = new Dictionary<GameStat, double>
            {
                { GameStat.Luck, initLuck },
                { GameStat.NewStageTileItemsForChoiceCount, initNewStageTileItemsForChoiceCount },
                { GameStat.NewStageRelicsForChoiceCount, initNewStageRelicsForChoiceCount },
                { GameStat.StageSpinsCount, initStageSpinsCount },
                { GameStat.AfterStageCoinsToDiamondsCoefficient, initAfterStageCoinsToDiamondsCoefficient },
                { GameStat.ShopPriceCoefficient, initShopPriceCoefficient },
                { GameStat.TileItemsInShopCount, initTileItemsInShopCount },
                { GameStat.RelicsInShopCount, initRelicsInShopCount },
            };
            Values = new Dictionary<GameStat, double>(initialValues);
        }

        public double Luck => Values[GameStat.Luck];
        public int NewStageTileItemsForChoiceCount => (int)Values[GameStat.NewStageTileItemsForChoiceCount];
        public int NewStageRelicsForChoiceCount => (int)Values[GameStat.NewStageRelicsForChoiceCount];
        public short StageSpinsCount => (short)Values[GameStat.StageSpinsCount];
        public double AfterStageCoinsToDiamondsCoefficient => Values[GameStat.AfterStageCoinsToDiamondsCoefficient];
        public double ShopPriceCoefficient => Values[GameStat.ShopPriceCoefficient];
        public int TileItemsInShopCount => (int)Values[GameStat.TileItemsInShopCount];
        public int RelicsInShopCount => (int)Values[GameStat.RelicsInShopCount];

        public void SetChanged() => Changed = true;

        public void Update(IEnumerable<GameStatRelicEffect> relicEffects)
        {
            var sortedEffects = relicEffects
                .OrderBy(effect => effect.ModifierType == ModifierType.Multiply ? 1 : 0)
                .ToList();

            Changed = false;
            foreach (var gameStat in Values.Keys)
                Values[gameStat] = initialValues[gameStat];
            foreach (var effect in sortedEffects)
            {
                Values[effect.Stat] = effect.ApplyEffect(Values[effect.Stat]);
            }
        }
    }

}
