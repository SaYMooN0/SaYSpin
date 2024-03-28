using SaYSpin.src.enums;
using SaYSpin.src.gameplay_parts;
using SaYSpin.src.gameplay_parts.inventory_related.relics;

namespace SaYSpin.src.extension_classes
{
    public static class GameFlowControllerExtensions
    {
        public static int CoinsNeededToCompleteTheStageCoefficient(this GameFlowController game) =>
            (int)CalculateCoefficient(game, GameStat.CoinsToDiamondsCoefficient, 1);
        public static double CoinsToDiamondsCoefficient(this GameFlowController game) =>
            CalculateCoefficient(game, GameStat.CoinsToDiamondsCoefficient, 1);
        private static double CalculateCoefficient(GameFlowController game, GameStat targetStat, double initialCoefficient)
        {
            foreach (var relic in game.Inventory.Relics.OfType<RelicWithGameStatInfluence>()
                     .Where(r => r.GameStat == targetStat).OrderByModifierType())
            {
                initialCoefficient = initialCoefficient.Apply(relic.ModificationValue, relic.ModifierType);
            }
            return initialCoefficient;
        }

    }


}
