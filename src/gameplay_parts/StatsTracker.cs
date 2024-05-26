
using SaYSpin.src.enums;
using SaYSpin.src.inventory_items.relics.relic_effects;
using System.Text.Json.Serialization;

namespace SaYSpin.src.gameplay_parts
{
    public class StatsTracker
    {

        public Dictionary<GameStat, double> Values { get; private set; }

        [JsonInclude]
        private readonly Dictionary<GameStat, double> initialValues;

        public bool Changed { get; private set; } = true;

        [JsonConstructor]
        public StatsTracker(Dictionary<GameStat, double> initialValues)
        {
            this.initialValues = initialValues;
            SetChanged();
        }
        public StatsTracker(
            double initLuck,
            int initNewStageTileItemsForChoiceCount,
            int initNewStageRelicsForChoiceCount,
            short initStageSpinsCount,
            double initAfterStageCoinsToDiamondsCoefficient,
            double initShopPriceCoefficient,
            int initTileItemsInShopCount,
            int initRelicsInShopCount,
            double initCoinsNeededToCompleteStage
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
                { GameStat.CoinsNeededToCompleteStage, initCoinsNeededToCompleteStage }
            };
        }

        public double Luck => Values[GameStat.Luck];
        public int NewStageTileItemsForChoiceCount => (int)Values[GameStat.NewStageTileItemsForChoiceCount];
        public int NewStageRelicsForChoiceCount => (int)Values[GameStat.NewStageRelicsForChoiceCount];
        public short StageSpinsCount => (short)Values[GameStat.StageSpinsCount];
        public double AfterStageCoinsToDiamondsCoefficient => Values[GameStat.AfterStageCoinsToDiamondsCoefficient];
        public double ShopPriceCoefficient => Values[GameStat.ShopPriceCoefficient];
        public int TileItemsInShopCount => (int)Values[GameStat.TileItemsInShopCount];
        public int RelicsInShopCount => (int)Values[GameStat.RelicsInShopCount];
        public double CoinsNeededToCompleteStage => Values[GameStat.CoinsNeededToCompleteStage];

        public void SetChanged() => Changed = true;

        public void Update(IEnumerable<GameStatRelicEffect> relicEffects)
        {
            var sortedEffects = relicEffects
                .OrderBy(effect => effect.ModifierType == ModifierType.Multiply ? 1 : 0)
                .ToList();

            ResetGameStats();

            foreach (var effect in sortedEffects)
            {
                Values[effect.Stat] = effect.ApplyEffect(Values[effect.Stat]);
            }
        }
        private void ResetGameStats()
        {
            Changed = false;
            foreach (var gameStat in initialValues.Keys)
                Values[gameStat] = initialValues[gameStat];
        }
    }

}
