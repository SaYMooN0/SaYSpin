using SaYSpin.src.abstract_classes;
using SaYSpin.src.gameplay_parts.inventory_related;
using System.Text;

namespace SaYSpin.src.gameplay_parts
{
    public class SlotMachine
    {
        public int RowsCount => SlotItems.GetLength(0);
        public int ColumnsCount => SlotItems.GetLength(1);
        public int TotalSlots => SlotItems.Length;
        public BaseTileItem?[,] SlotItems { get; private set; }

        public SlotMachine(List<BaseTileItem> startingTiles, int rowsCount = 3, int columnsCount = 3)
        {
            SlotItems = new BaseTileItem?[rowsCount, columnsCount];
            UpdateItems(startingTiles);
        }

        public void UpdateItems(TileItemsPicker itemsPicker) =>
            UpdateItems(itemsPicker.GetRandomizedItems());

        private void UpdateItems(List<BaseTileItem> newItems)
        {
            Random rand = new Random();
            var positions = Enumerable.Range(0, TotalSlots)
                                      .OrderBy(x => rand.Next())
                                      .ToList();

            SlotItems = new BaseTileItem?[RowsCount, ColumnsCount];

            for (int i = 0; i < newItems.Count; i++)
            {
                int pos = positions[i];
                int row = pos / ColumnsCount;
                int col = pos % ColumnsCount;
                SlotItems[row, col] = newItems[i];
            }
        }

        public void IncreaseRowsCount()
        {
            var newSlotItems = new BaseTileItem?[RowsCount + 1, ColumnsCount];
            Array.Copy(SlotItems, 0, newSlotItems, 0, SlotItems.Length);
            SlotItems = newSlotItems;
        }

        public void IncreaseColumnsCount()
        {
            var newSlotItems = new BaseTileItem?[RowsCount, ColumnsCount + 1];
            for (int row = 0; row < RowsCount; row++)
            {
                for (int column = 0; column < ColumnsCount; column++)
                {
                    newSlotItems[row, column] = SlotItems[row, column];
                }
            }
            SlotItems = newSlotItems;
        }

        public override string ToString()
        {
            StringBuilder sB = new StringBuilder();

            int emptyCount = 0;
            int filledCount = 0;

            int maxStringLength = SlotItems.Cast<BaseTileItem?>()
                                           .Where(item => item is not null)
                                           .Max(item => item.ToString().Length);

            string formatString = $"{{0, -{maxStringLength + 2}}}";

            for (int i = 0; i < RowsCount; i++)
            {
                for (int j = 0; j < ColumnsCount; j++)
                {
                    if (SlotItems[i, j] is null)
                    {
                        emptyCount++;
                        sB.AppendFormat(formatString, "null");
                    }
                    else
                    {
                        filledCount++;
                        sB.AppendFormat(formatString, $"\"{SlotItems[i, j]}\"");
                    }
                }
                sB.AppendLine();
            }

            sB.AppendLine($"Rows: {RowsCount}   Columns: {ColumnsCount}     Empty: {emptyCount}   Filled: {filledCount}");
            return sB.ToString();
        }
    }
}
