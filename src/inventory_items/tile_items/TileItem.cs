using SaYSpin.src.enums;
using SaYSpin.src.gameplay_parts;

namespace SaYSpin.src.inventory_items.tile_items
{
    public class TileItem : BaseInventoryItem
    {
        public TileItem(string name, string description, Rarity rarity, int initialCoinValue, string[] tags, HashSet<BaseTileItemEffect> effects, CalculateIncomeDelegate incomeCalculationFunc)
            : base(name, description, rarity)
        {
            Tags = tags;
            InitialCoinValue = initialCoinValue;
            Effects = effects;
            CalculateIncome = incomeCalculationFunc;
        }

        public string[] Tags { get; init; }
        public int InitialCoinValue { get; init; }
        public HashSet<BaseTileItemEffect> Effects { get; init; } = new();
        public CalculateIncomeDelegate CalculateIncome { get; init; }
        public override string ImageFolderPath => "tile_items";
        public override string ToString() =>
            $"{{Id: {Id}, Rarity: {Rarity}, InitialCoinValue: {InitialCoinValue}}}";

        public delegate int CalculateIncomeDelegate(IEnumerable<TileItemIncomeBonus> bonuses);
        public TileItem WithEffect(BaseTileItemEffect effect)
        {
            Effects.Add(effect);
            Description += (string.IsNullOrEmpty(Description) ? "" : "\n") + effect.Description;
            return this;
        }
        public static TileItem Ordinary(string name, Rarity rarity, int initialCoinValue, string[] tags) =>
           new(name, $"Gives {initialCoinValue} coins with each drop", rarity, initialCoinValue, tags, [],
               (IEnumerable<TileItemIncomeBonus> bonuses) =>
               {
                   double value = initialCoinValue;
                   foreach (var b in bonuses.OrderByModifierType())
                   {
                       value = value.Apply(b.ModifierValue, b.ModifierType);
                   }
                   return (int)value;
               });
    }
}
