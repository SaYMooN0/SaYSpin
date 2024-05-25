using SaYSpin.src.gameplay_parts;
using SaYSpin.src.inventory_items;
using SaYSpin.src.inventory_items.tile_items;

namespace SaYSpin.src.game_saving.dtos
{
    internal class SlotMachineDTO
    {
        public int RowsCount { get; set; }
        public int ColumnsCount { get; set; }
        public List<InventoryItemDTO?> TileItems { get; set; } = new();

        public static SlotMachineDTO FromSlotMachine(SlotMachine slotMachine)
        {
            var dto = new SlotMachineDTO
            {
                RowsCount = slotMachine.RowsCount,
                ColumnsCount = slotMachine.ColumnsCount
            };

            for (int row = 0; row < slotMachine.RowsCount; row++)
            {
                for (int col = 0; col < slotMachine.ColumnsCount; col++)
                {
                    var tileItem = slotMachine.TileItems[row, col];
                    dto.TileItems.Add(
                        tileItem is not null ?
                        new InventoryItemDTO { Name = tileItem.Name, Counter = tileItem is IWithCounter withCounter ? withCounter.Counter : null }
                        : null);
                }
            }

            return dto;
        }

        public SlotMachine ToSlotMachine(IDictionary<string, Func<TileItem>> tileItemConstructors)
        {
            var tileItems = new List<TileItem>();
            foreach (var dto in TileItems)
            {
                if (dto is InventoryItemDTO tiDto)
                    tileItems.Add(tiDto.ToItem(tileItemConstructors));
                else
                    tileItems.Add(null);
            }

            var slotMachine = new SlotMachine(tileItems, RowsCount, ColumnsCount);
            return slotMachine;
        }
    }

}
