using SaYSpin.src.gameplay_parts;
using SaYSpin.src.inventory_items.tile_items;

namespace SaYSpin.src.game_saving.dtos
{
    internal class SlotMachineDTO
    {
        public int RowsCount { get; set; }
        public int ColumnsCount { get; set; }
        public List<TileItemDTO?> TileItems { get; set; } = new List<TileItemDTO?>();

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
                    dto.TileItems.Add(tileItem != null ? new TileItemDTO { Name = tileItem.Name, Counter = tileItem is TileItemWithCounter tiWithCounter ? tiWithCounter.Counter : (int?)null } : null);
                }
            }

            return dto;
        }

        public SlotMachine ToSlotMachine(IDictionary<string, Func<TileItem>> tileItemConstructors)
        {
            var tileItems = new List<TileItem>();
            foreach (var dto in TileItems)
            {
                if (dto is TileItemDTO tiDto)
                    tileItems.Add(tiDto.ToTileItem(tileItemConstructors));
                else
                    tileItems.Add(null);
            }

            var slotMachine = new SlotMachine(tileItems, RowsCount, ColumnsCount);
            return slotMachine;
        }
    }

}
