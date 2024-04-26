namespace SaYSpin.src.inventory_items.tile_items
{
    public abstract class BaseTileItemEffect
    {
        public string Description { get; init; }
        public TileItem SourceTileItem { get; init; }
        protected BaseTileItemEffect(string description, TileItem sourceTileItem)
        {
            Description = description;
            SourceTileItem = sourceTileItem;
        }
    }
}
