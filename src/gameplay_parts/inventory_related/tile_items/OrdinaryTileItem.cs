using SaYSpin.src.coins_calculation_related;
using SaYSpin.src.enums;
using SaYSpin.src.inventory_items.tile_items;

namespace SaYSpin.src.gameplay_parts.inventory_related.tile_items
{
    public class OrdinaryTileItem : BaseTileItem
    {
        public OrdinaryTileItem(string name, Rarity rarity, int initialCoinValue, string[]? tags)
            : base(name, $"Gives {initialCoinValue} coins with each drop", rarity, initialCoinValue, tags ?? Array.Empty<string>()) { }

        public override int CalculateIncome(IEnumerable<TileItemIncomeBonus> bonuses)
        {
            double value = InitialCoinValue;
            foreach (var b in bonuses.OrderByModifierType())
            {
                value = value.Apply(b.ModifierValue, b.ModifierType);
            }
            return (int)value;
        }
    }
}
