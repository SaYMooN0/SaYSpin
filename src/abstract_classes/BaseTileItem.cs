using SaYSpin.src.gameplay_parts;

namespace SaYSpin.src.abstract_classes
{
    public abstract class BaseTileItem : BaseInventoryItem
    {
        protected BaseTileItem(Rarity rarity, int coinValue)
        {
            Rarity = rarity;
            CoinValue = coinValue;
        }

        public override string ImageFolderPath => "tile_items";
        public int CoinValue { get; init; }
        public override string ToString() => $"{{Id: {Id}, CoinValue: {CoinValue}}}";
    }
}
