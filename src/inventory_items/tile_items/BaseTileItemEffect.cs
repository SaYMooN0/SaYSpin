namespace SaYSpin.src.inventory_items.tile_items
{
    public abstract class BaseTileItemEffect
    {
        public string Description { get; init; }
        protected BaseTileItemEffect(string description)
        {
            Description = description;
        }
    }
}
