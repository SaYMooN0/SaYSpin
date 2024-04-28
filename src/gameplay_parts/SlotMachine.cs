using SaYSpin.src.enums;
using SaYSpin.src.inventory_items.relics.relic_effects;
using SaYSpin.src.inventory_items.tile_items;
using SaYSpin.src.secondary_classes;
using SaYSpin.src.tile_item_effects;
using System.Text;

namespace SaYSpin.src.gameplay_parts
{
    public class SlotMachine
    {
        public int RowsCount => TileItems.GetLength(0);
        public int ColumnsCount => TileItems.GetLength(1);
        public int TotalSlots => TileItems.Length;
        public TileItem?[,] TileItems { get; private set; }

        public SlotMachine(List<TileItem> startingTiles, int rowsCount = 3, int columnsCount = 3)
        {
            TileItems = new TileItem?[rowsCount, columnsCount];
            UpdateItems(startingTiles);
        }

        public void UpdateItems(TileItemsPicker itemsPicker) =>
            UpdateItems(itemsPicker.GetRandomizedItems());

        private void UpdateItems(List<TileItem> newItems)
        {
            Random rand = new();
            var positions = Enumerable.Range(0, TotalSlots)
                                      .OrderBy(x => rand.Next())
                                      .ToList();

            TileItems = new TileItem?[RowsCount, ColumnsCount];

            for (int i = 0; i < newItems.Count; i++)
            {
                int pos = positions[i];
                int row = pos / ColumnsCount;
                int col = pos % ColumnsCount;
                TileItems[row, col] = newItems[i];
            }
        }
        public void IncreaseRowsCount()
        {
            var newSlotItems = new TileItem?[RowsCount + 1, ColumnsCount];
            Array.Copy(TileItems, 0, newSlotItems, 0, TileItems.Length);
            TileItems = newSlotItems;
        }

        public void IncreaseColumnsCount()
        {
            var newSlotItems = new TileItem?[RowsCount, ColumnsCount + 1];
            for (int row = 0; row < RowsCount; row++)
            {
                for (int column = 0; column < ColumnsCount; column++)
                {
                    newSlotItems[row, column] = TileItems[row, column];
                }
            }
            TileItems = newSlotItems;
        }
        public IEnumerable<TileItem?> SingleDimensionTileItems()
        {
            for (int row = 0; row < TileItems.GetLength(0); row++)
            {
                for (int col = 0; col < TileItems.GetLength(1); col++)
                {
                    yield return TileItems[row, col];
                }
            }
        }
        public void SetTileItemNull(int row, int col)
        {
            if (row >= 0 && row < RowsCount && col >= 0 && col < ColumnsCount)
                TileItems[row, col] = null;
        }
        public override string ToString()
        {
            StringBuilder sB = new();

            int emptyCount = 0;
            int filledCount = 0;
            int maxItemLength = TileItems.Cast<TileItem?>()
                                         .Where(item => item is not null)
                                         .Max(item => item?.ToString().Length ?? 0) + ColumnsCount;

            string formatString = $"{{0, -{maxItemLength}}}";
            int totalLineLength = (maxItemLength + 1) * ColumnsCount;

            for (int i = 0; i < RowsCount; i++)
            {
                for (int j = 0; j < ColumnsCount; j++)
                {
                    if (TileItems[i, j] is null)
                    {
                        emptyCount++;
                        sB.AppendFormat(formatString, "|null");
                    }
                    else
                    {
                        filledCount++;
                        sB.AppendFormat(formatString, $"|{TileItems[i, j]}");
                    }
                }
                sB.Append("|\n").AppendLine(string.Concat(Enumerable.Repeat("-", totalLineLength)));
            }

            sB.AppendLine($"Rows: {RowsCount}\tColumns: {ColumnsCount}\tEmpty: {emptyCount}\tFilled: {filledCount}");
            return sB.ToString();
        }


    }
}
