namespace SaYSpin.src.abstract_classes
{
    public abstract class BaseTileItem : BaseInventoryItem
    {
        public override string ImageFolderPath => "tile_items";
        public int CoinValue { get; init; }
        public override string ToString() => $"{{Id: {Id}, CoinValue: {CoinValue}}}";
    }
}
