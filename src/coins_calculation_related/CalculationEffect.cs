using SaYSpin.src.abstract_classes;

namespace SaYSpin.src.coins_calculation_related
{
    public delegate double CoinsEffectDelegate(BaseTileItem item);
    public delegate bool TileItemEffectedDelegate(BaseTileItem item);

    public class CalculationEffect
    {
        public CoinsEffectDelegate CoinsEffect { get; init; }
        public TileItemEffectedDelegate TileItemEffected { get; init; }
        public List<CalculationEffectApplicationArea> AreasToApply { get; init; }
        public bool DestroyEffected { get; init; }
        public CalculationEffect(
            CoinsEffectDelegate coinsEffect, TileItemEffectedDelegate tileItemEffected,
            CalculationEffectApplicationArea areaToApply, bool destroyEffected)
            : this(coinsEffect, tileItemEffected, new List<CalculationEffectApplicationArea> { areaToApply }, destroyEffected) { }


        public CalculationEffect(
            CoinsEffectDelegate coinsEffect,
            TileItemEffectedDelegate tileItemEffected,
            List<CalculationEffectApplicationArea> areasToApply,
            bool destroyEffected)
        {
            CoinsEffect = coinsEffect;
            TileItemEffected = tileItemEffected;
            AreasToApply = areasToApply;
            DestroyEffected = destroyEffected;
        }
    }

    public enum CalculationEffectApplicationArea
    {
        Self,
        AllTiles
    }
}
