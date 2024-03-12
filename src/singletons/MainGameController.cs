using SaYSpin.src.gameplay_parts;
using SaYSpin.src.gameplay_parts.inventory_related;

namespace SaYSpin.src.singletons
{
    public class MainGameController
    {
        public SlotMachine SlotMachine { get; init; }
        public Inventory Inventory { get; init; }
        public MainGameController()
        {
            OrdinaryTileItem t1 = new("i:1", "1.png", 1);
            OrdinaryTileItem t2 = new("i:2", "2.png", 2);
            OrdinaryTileItem t3 = new("i:3", "3.png", 3);

            Inventory = new([t1, t2, t3]);
            SlotMachine = new(Inventory.TileItems, 3, 3);
            Logger.Log(SlotMachine);


            TileItemsPicker itemPicker = new(SlotMachine.TotalSlots);
            itemPicker.PickItemsFrom(Inventory.TileItems);
            SlotMachine.UpdateItems(itemPicker);
            Logger.Log(SlotMachine);

            itemPicker = new(SlotMachine.TotalSlots);
            itemPicker.PickItemsFrom(Inventory.TileItems);
            SlotMachine.UpdateItems(itemPicker);
            Logger.Log(SlotMachine);
        }
    }
}
