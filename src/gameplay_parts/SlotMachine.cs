﻿using SaYSpin.src.coins_calculation_related;
using SaYSpin.src.enums;
using SaYSpin.src.inventory_items.tile_items;
using SaYSpin.src.relic_effects;
using SaYSpin.src.secondary_classes;
using System.Text;

namespace SaYSpin.src.gameplay_parts
{
    public class SlotMachine
    {
        public int RowsCount => TileItems.GetLength(0);
        public int ColumnsCount => TileItems.GetLength(1);
        public int TotalSlots => TileItems.Length;
        public BaseTileItem?[,] TileItems { get; private set; }

        public SlotMachine(List<BaseTileItem> startingTiles, int rowsCount = 3, int columnsCount = 3)
        {
            TileItems = new BaseTileItem?[rowsCount, columnsCount];
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

            TileItems = new BaseTileItem?[RowsCount, ColumnsCount];

            for (int i = 0; i < newItems.Count; i++)
            {
                int pos = positions[i];
                int row = pos / ColumnsCount;
                int col = pos % ColumnsCount;
                TileItems[row, col] = newItems[i];
            }
        }
        public int CalculateCoinValue(List<CoinsCalculationEffect> relicEffects)
        {

            TileItemBonusesGrid bonuses = GatherCalculationBonuses(relicEffects);
            int coinValue = 0;
            for (int i = 0; i < TileItems.GetLength(0); i++)
            {
                for (int j = 0; j < TileItems.GetLength(1); j++)
                {
                    var item = TileItems[i, j];
                    if (item != null)
                    {
                        coinValue += item.CalculateIncome(
                            bonuses.GetBonusesFor(i, j));
                    }
                }
            }

            return coinValue;
        }
        private TileItemBonusesGrid GatherCalculationBonuses(List<CoinsCalculationEffect> relicEffects)
        {

            TileItemBonusesGrid bonuses = new(TileItems.GetLength(0), TileItems.GetLength(1));

            if (relicEffects is null || relicEffects.Count == 0)
                return bonuses;


            for (int i = 0; i < TileItems.GetLength(0); i++)
            {
                for (int j = 0; j < TileItems.GetLength(1); j++)
                {
                    var tileItem = TileItems[i, j];
                    if (tileItem is null)
                        continue;

                    ApplyRelicEffectsToTileItemInGrid(relicEffects, bonuses, i, j, tileItem);
                }
            }

            return bonuses;
        }

        private void ApplyRelicEffectsToTileItemInGrid(List<CoinsCalculationEffect> relicEffects, TileItemBonusesGrid bonuses, int i, int j, BaseTileItem tileItem)
        {
            foreach (var rEffect in relicEffects)
            {
                if (!rEffect.Condition(tileItem)) continue;

                switch (rEffect.Area)
                {
                    case EffectApplicationArea.Self:
                        bonuses.AddBonus(i, j, new TileItemIncomeBonus(rEffect.ModifierType, rEffect.ModificationValue));
                        break;
                    default:
                        throw new NotImplementedException("EffectApplicationArea Unsupported type");
                }
            }
        }

        public void IncreaseRowsCount()
        {
            var newSlotItems = new BaseTileItem?[RowsCount + 1, ColumnsCount];
            Array.Copy(TileItems, 0, newSlotItems, 0, TileItems.Length);
            TileItems = newSlotItems;
        }

        public void IncreaseColumnsCount()
        {
            var newSlotItems = new BaseTileItem?[RowsCount, ColumnsCount + 1];
            for (int row = 0; row < RowsCount; row++)
            {
                for (int column = 0; column < ColumnsCount; column++)
                {
                    newSlotItems[row, column] = TileItems[row, column];
                }
            }
            TileItems = newSlotItems;
        }
        public IEnumerable<BaseTileItem?> SingleDimensionTileItems()
        {
            for (int row = 0; row < TileItems.GetLength(0); row++)
            {
                for (int col = 0; col < TileItems.GetLength(1); col++)
                {
                    yield return TileItems[row, col];
                }
            }
        }
        public override string ToString()
        {
            StringBuilder sB = new StringBuilder();

            int emptyCount = 0;
            int filledCount = 0;
            int maxItemLength = TileItems.Cast<BaseTileItem?>()
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
