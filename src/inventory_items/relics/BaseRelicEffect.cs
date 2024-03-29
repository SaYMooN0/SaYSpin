namespace SaYSpin.src.inventory_items.relics
{
    public abstract class BaseRelicEffect
    {
        public string Description { get; init; }
        protected BaseRelicEffect(string description) { Description = description; }
    }
}
