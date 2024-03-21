using SaYSpin.src.gameplay_parts;

namespace SaYSpin.src.abstract_classes
{
    public abstract class BaseTileItem : BaseInventoryItem
    {
        protected BaseTileItem(string id, string name, string description, string image, Rarity rarity, int coinValue, string[] tags)
            : base(id, name, description, image, rarity)
        {
            Tags = tags;
            CoinValue = coinValue;
        }

        public string[] Tags { get; init; }
        public int CoinValue { get; init; }

        public override string ImageFolderPath => "tile_items";
        public override string ToString() => $"{{Id: {Id}, CoinValue: {CoinValue}}}";
    }
}
