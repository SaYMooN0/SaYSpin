using SaYSpin.src.gameplay_parts;

namespace SaYSpin.src.singletons
{
    public class MainGameController
    {
        public SlotMachine SlotMachine { get; init; }
        public Inventory Inventory { get; init; }
        public MainGameController()
        {
            SlotMachine = new();
            Inventory = new();
        }
    }
}
