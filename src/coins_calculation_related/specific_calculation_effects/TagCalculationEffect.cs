
namespace SaYSpin.src.coins_calculation_related.specific_calculation_effects
{
    public class TagCalculationEffect : CalculationEffect
    {
        public TagCalculationEffect(CoinsEffectDelegate coinsEffect, TileItemEffectedDelegate tileItemEffected)
            : base(coinsEffect, tileItemEffected, CalculationEffectApplicationArea.AllTiles, false)
        {
        }
    }
}
