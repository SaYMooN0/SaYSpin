using SaYSpin.src.coins_calculation_related;
using SaYSpin.src.gameplay_parts;

namespace SaYSpin.src.abstract_classes
{
    public abstract class BaseTileItem : BaseInventoryItem
    {
        protected BaseTileItem(string name, string description, Rarity rarity, int initialCoinValue, string[] tags)
            : base(name, description, rarity)
        {
            Tags = tags;
            InitialCoinValue = initialCoinValue;
        }

        public string[] Tags { get; init; }
        public int InitialCoinValue { get; init; }
        public override string ImageFolderPath => "tile_items";
        public override string ToString() => 
            $"{{Id: {Id}, Rarity: {Rarity}, InitialCoinValue: {InitialCoinValue}}}";

        public abstract int CalculateIncome(IEnumerable<TileItemIncomeBonus> bonuses);
    }
}
