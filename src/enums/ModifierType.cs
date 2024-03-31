using SaYSpin.src.enums;
using SaYSpin.src.inventory_items.tile_items;
namespace SaYSpin.src.enums
{
    public enum ModifierType
    {
        Multiply,
        Plus
    }
    public static class ModifierTypeSortingExtension
    {
        //public static IEnumerable<RelicWithGameStatInfluence> OrderByModifierType(this IEnumerable<RelicWithGameStatInfluence> source) =>
        //    source.OrderBy(relic => relic.ModifierType == ModifierType.Plus ? 0 : 1);
        public static IEnumerable<TileItemIncomeBonus> OrderByModifierType(this IEnumerable<TileItemIncomeBonus> source) =>
            source.OrderBy(bonus => bonus.ModifierType == ModifierType.Plus ? 0 : 1);
        public static double Apply(this int baseValue, int modifierValue, ModifierType modifierType)
        {
            return modifierType switch
            {
                ModifierType.Plus => baseValue + modifierValue,
                ModifierType.Multiply => baseValue * modifierValue,
                _ => baseValue
            };
        }
        public static double Apply(this double baseValue, double modifierValue, ModifierType modifierType)
        {
            return modifierType switch
            {
                ModifierType.Plus => baseValue + modifierValue,
                ModifierType.Multiply => baseValue * modifierValue,
                _ => baseValue
            };
        }
    }
}
