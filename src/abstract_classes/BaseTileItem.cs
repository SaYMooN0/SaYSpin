using SaYSpin.src.gameplay_parts;

namespace SaYSpin.src.abstract_classes
{
    public abstract class BaseTileItem : BaseInventoryItem
    {
        public BaseTileItem(string id, string name, string description, string image, Rarity rarity, int coinValue) : base(id, name, description, image, rarity)
        {
            CoinValue = coinValue;
        }

        public override string ImageFolderPath => "tile_items";
        public int CoinValue { get; init; }
        public override string ToString() => $"{{Id: {Id}, CoinValue: {CoinValue}}}";
    }
}
