using SaYSpin.src.enums;
using SaYSpin.src.gameplay_parts;
using System.Text;

namespace SaYSpin.src.inventory_items.tile_items
{
    public class TileItem : BaseInventoryItem
    {
        public TileItem(string name, string description, Rarity rarity, int initialCoinValue, string[] tags, HashSet<BaseTileItemEffect> effects, CalculateIncomeDelegate incomeCalculationFunc,
            bool isSpecial = false, bool isUnique = false)
            : base(name, description, rarity, isSpecial, isUnique)
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
        public int LastIncome { get; private set; }
        public void SetLastIncomeToZero() =>
            LastIncome = 0;
        public void SetLastIncome(int value) =>
            LastIncome = value;
        public override string ImageFolderPath => "tile_items";
        public override string ToString() =>
            $"{{Id: {Id}, Rarity: {Rarity}, InitialCoinValue: {InitialCoinValue}}}";

        public delegate int CalculateIncomeDelegate(IEnumerable<TileItemIncomeBonus> bonuses);
        public TileItem WithEffect(BaseTileItemEffect effect)
        {
            Effects.Add(effect);
            if (!string.IsNullOrEmpty(effect.Description))
                Description += (string.IsNullOrEmpty(Description) ? "" : "\n") + effect.Description;
            return this;
        }
        public static TileItem Ordinary(string name, Rarity rarity, int initialCoinValue, string[] tags) =>
           new(name, $"Gives {initialCoinValue} coins", rarity, initialCoinValue, tags, [], (bonuses) => _basciIncomeCalculationFunc(bonuses, initialCoinValue));
        public static TileItem Special(string name, Rarity rarity, int initialCoinValue, string[] tags) =>
           new(name, $"Gives {initialCoinValue} coins", rarity, initialCoinValue, tags, [], (bonuses) => _basciIncomeCalculationFunc(bonuses, initialCoinValue), isSpecial: true);
        public static TileItem Unique(string name, Rarity rarity, int initialCoinValue, string[] tags) =>
           new(name, $"Gives {initialCoinValue} coins", rarity, initialCoinValue, tags, [], (bonuses) => _basciIncomeCalculationFunc(bonuses, initialCoinValue),isUnique: true);
        public override string TextInfo()
        {
            StringBuilder sb = new();
            sb.Append($"Name: {Name}\nRarity: {Rarity}\nId: {Id}\nCoinValue: {InitialCoinValue}\n");
            if (Effects.Count < 1)
            {
                sb.Append("No effects");
                return sb.ToString();
            }
            sb.Append("Effects:\n");
            for (int i = 0; i < Effects.Count; i++)
            {
                sb.Append($"{i + 1}. {Effects.ElementAt(i).Description}\n");
            }
            return sb.ToString();
        }
        private static int _basciIncomeCalculationFunc(IEnumerable<TileItemIncomeBonus> bonuses, int initialCoinValue)
        {
            double value = initialCoinValue;
            foreach (var b in bonuses.OrderByModifierType())
            {
                value = value.Apply(b.ModifierValue, b.ModifierType);
            }
            return (int)value;
        }
    }
}
