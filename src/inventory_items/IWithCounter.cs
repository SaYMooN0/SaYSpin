namespace SaYSpin.src.inventory_items
{
    public interface IWithCounter
    {
        int Counter { get; }

        void IncrementCounter(int amount);
        void ResetCounter();
    }
}
