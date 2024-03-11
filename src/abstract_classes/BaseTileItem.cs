namespace SaYSpin.src.abstract_classes
{
    public abstract class BaseTileItem : BaseGameplayItem
    {
        public override string ImageFolderPath => "tile_items";
        public int CoinValue;
        public override string ToString() => $"Id: {Id}, CoinValue: {CoinValue}";
    }
}
