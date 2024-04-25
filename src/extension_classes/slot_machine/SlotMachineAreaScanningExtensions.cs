
using SaYSpin.src.enums;
using SaYSpin.src.gameplay_parts;
using SaYSpin.src.inventory_items.tile_items;

namespace SaYSpin.src.extension_classes.slot_machine
{
    internal static class SlotMachineAreaScanningExtensions
    {
        public static IEnumerable<(int row, int col)> GetTileItemCoordinates(this SlotMachine slotMachine, int i, int j, SlotMachineArea area, Func<TileItem?, bool> condition = null)
        {
            condition ??= (TileItem? ti) => ti is not null;

            switch (area)
            {
                case SlotMachineArea.Adjacent:
                    return GetAdjacentCoordinates(slotMachine, i, j, condition);
                case SlotMachineArea.Square3:
                    return GetSquareCoordinates(slotMachine, i, j, 1, condition);
                case SlotMachineArea.Square5:
                    return GetSquareCoordinates(slotMachine, i, j, 2, condition);
                case SlotMachineArea.AllTiles:
                    return GetAllTileCoordinates(slotMachine, condition);
                case SlotMachineArea.HorizontalLine:
                    return GetLineCoordinates(slotMachine, i, j, true, condition);
                case SlotMachineArea.VerticalLine:
                    return GetLineCoordinates(slotMachine, i, j, false, condition);
                case SlotMachineArea.CornerTiles:
                    return GetCornerCoordinates(slotMachine, condition);
                default:
                    throw new NotImplementedException($"Area {area} is not implemented in the GetTileItemCoordinates method.");
            }
        }

        private static IEnumerable<(int, int)> GetAdjacentCoordinates(SlotMachine slotMachine, int i, int j, Func<TileItem?, bool> condition)
        {
            (int, int)[] offsets = [(-1, 0), (1, 0), (0, -1), (0, 1), (1, 1), (1, -1), (-1, 1), (-1, -1)];
            foreach (var (di, dj) in offsets)
            {
                int newI = i + di, newJ = j + dj;
                if (newI >= 0 && newI < slotMachine.TileItems.GetLength(0) && newJ >= 0 && newJ < slotMachine.TileItems.GetLength(1))
                    if (condition(slotMachine.TileItems[newI, newJ]))
                        yield return (newI, newJ);
            }
        }

        private static IEnumerable<(int, int)> GetSquareCoordinates(SlotMachine slotMachine, int i, int j, int radius, Func<TileItem?, bool> condition)
        {
            for (int di = -radius; di <= radius; di++)
                for (int dj = -radius; dj <= radius; dj++)
                    if (i + di >= 0 && i + di < slotMachine.TileItems.GetLength(0) && j + dj >= 0 && j + dj < slotMachine.TileItems.GetLength(1))
                        if (condition(slotMachine.TileItems[i + di, j + dj]))
                            yield return (i + di, j + dj);
        }

        private static IEnumerable<(int, int)> GetAllTileCoordinates(SlotMachine slotMachine, Func<TileItem?, bool> condition)
        {
            for (int i = 0; i < slotMachine.TileItems.GetLength(0); i++)
                for (int j = 0; j < slotMachine.TileItems.GetLength(1); j++)
                    if (condition(slotMachine.TileItems[i, j]))
                        yield return (i, j);
        }

        private static IEnumerable<(int, int)> GetLineCoordinates(SlotMachine slotMachine, int i, int j, bool isHorizontal, Func<TileItem?, bool> condition)
        {
            if (isHorizontal)
            {
                for (int dj = 0; dj < slotMachine.TileItems.GetLength(1); dj++)
                    if (condition(slotMachine.TileItems[i, dj]))
                        yield return (i, dj);
            }
            else
            {
                for (int di = 0; di < slotMachine.TileItems.GetLength(0); di++)
                    if (condition(slotMachine.TileItems[di, j]))
                        yield return (di, j);
            }
        }

        private static IEnumerable<(int, int)> GetCornerCoordinates(SlotMachine slotMachine, Func<TileItem?, bool> condition)
        {
            var corners = new[] { (0, 0), (0, slotMachine.TileItems.GetLength(1) - 1), (slotMachine.TileItems.GetLength(0) - 1, 0), (slotMachine.TileItems.GetLength(0) - 1, slotMachine.TileItems.GetLength(1) - 1) };
            foreach (var (ci, cj) in corners)
                if (condition(slotMachine.TileItems[ci, cj]))
                    yield return (ci, cj);
        }
    }
}
