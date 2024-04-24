using SaYSpin.src.enums;
using SaYSpin.src.gameplay_parts;
using SaYSpin.src.inventory_items.relics.relic_effects;
using SaYSpin.src.inventory_items.tile_items;
using SaYSpin.src.secondary_classes;
using SaYSpin.src.tile_item_effects;


namespace SaYSpin.src.extension_classes
{
    public static class SlotMachineCalculationExtensions
    {
        public static int CalculateCoinValue(this SlotMachine slotMachine, IEnumerable<CoinsCalculationRelicEffect> relicEffects)
        {
            TileItemBonusesGrid bonuses = slotMachine.GatherCalculationBonuses(relicEffects);
            int coinValue = 0;
            for (int i = 0; i < slotMachine.TileItems.GetLength(0); i++)
            {
                for (int j = 0; j < slotMachine.TileItems.GetLength(1); j++)
                {
                    var item = slotMachine.TileItems[i, j];
                    if (item is not null)
                    {
                        int income= item.CalculateIncome(bonuses.GetBonusesFor(i, j));
                        coinValue += income;
                        item.SetLastIncome(income);
                        
                    }
                }
            }

            return coinValue;
        }

        private static TileItemBonusesGrid GatherCalculationBonuses(this SlotMachine slotMachine, IEnumerable<CoinsCalculationRelicEffect> relicEffects)
        {
            TileItemBonusesGrid bonusesGrid = new TileItemBonusesGrid(slotMachine.TileItems.GetLength(0), slotMachine.TileItems.GetLength(1));

            for (int i = 0; i < slotMachine.TileItems.GetLength(0); i++)
            {
                for (int j = 0; j < slotMachine.TileItems.GetLength(1); j++)
                {
                    var tileItem = slotMachine.TileItems[i, j];
                    if (tileItem is null)
                        continue;

                    bonusesGrid.ApplyRelicEffectsToTileItem(relicEffects, i, j, tileItem);

                    var enhancingBonuses = tileItem.Effects.OfType<TileItemsEnhancingTileItemEffect>();
                    foreach (var eEffect in enhancingBonuses)
                    {
                        slotMachine.ApplyEffectBasedOnArea(bonusesGrid, i, j, eEffect);
                    }
                }
            }

            return bonusesGrid;
        }

        private static void ApplyEffectBasedOnArea(this SlotMachine slotMachine, TileItemBonusesGrid bonusesGrid, int i, int j, TileItemsEnhancingTileItemEffect effect)
        {
            switch (effect.Area)
            {
                case SlotMachineArea.Self:
                    if (effect.Condition(slotMachine.TileItems[i, j]))
                    {
                        bonusesGrid.AddBonus(i, j, new TileItemIncomeBonus(effect.ModifierType, effect.ModificationValue));
                    }
                    break;
                case SlotMachineArea.Adjacent:
                    slotMachine.AddBonusToAdjacent(bonusesGrid, i, j, effect);
                    break;
                case SlotMachineArea.Square3:
                case SlotMachineArea.Square5:
                    slotMachine.AddBonusToSquare(bonusesGrid, i, j, effect, effect.Area == SlotMachineArea.Square3 ? 3 : 5);
                    break;
                case SlotMachineArea.AllTiles:
                    slotMachine.AddBonusToAllTiles(bonusesGrid, effect);
                    break;
                case SlotMachineArea.HorizontalLine:
                    slotMachine.AddBonusToHorizontalLine(bonusesGrid, i, j, effect);
                    break;
                case SlotMachineArea.VerticalLine:
                    slotMachine.AddBonusToVerticalLine(bonusesGrid, i, j, effect);
                    break;
                case SlotMachineArea.CornerTiles:
                    slotMachine.AddBonusToCornerTiles(bonusesGrid, effect);
                    break;
                default:
                    throw new NotImplementedException($"EffectApplicationArea {effect.Area} is not implemented.");
            }
        }

        private static void AddBonusToHorizontalLine(this SlotMachine slotMachine, TileItemBonusesGrid bonusesGrid, int i, int sourceJ, TileItemsEnhancingTileItemEffect effect)
        {
            for (int j = 0; j < slotMachine.TileItems.GetLength(1); j++)
            {
                if (j == sourceJ) continue;
                if (effect.Condition(slotMachine.TileItems[i, j]))
                {
                    bonusesGrid.AddBonus(i, j, new TileItemIncomeBonus(effect.ModifierType, effect.ModificationValue));
                }
            }
        }

        private static void AddBonusToVerticalLine(this SlotMachine slotMachine, TileItemBonusesGrid bonusesGrid, int sourceI, int j, TileItemsEnhancingTileItemEffect effect)
        {
            for (int i = 0; i < slotMachine.TileItems.GetLength(0); i++)
            {
                if (i == sourceI) continue;
                if (effect.Condition(slotMachine.TileItems[i, j]))
                {
                    bonusesGrid.AddBonus(i, j, new TileItemIncomeBonus(effect.ModifierType, effect.ModificationValue));
                }
            }
        }


        private static void AddBonusToCornerTiles(this SlotMachine slotMachine, TileItemBonusesGrid bonusesGrid, TileItemsEnhancingTileItemEffect effect)
        {
            int[] cornerIndices = { 0, slotMachine.TileItems.GetLength(0) - 1 };
            foreach (int i in cornerIndices)
            {
                foreach (int j in cornerIndices)
                {
                    if (i < slotMachine.TileItems.GetLength(0) && j < slotMachine.TileItems.GetLength(1) &&
                        effect.Condition(slotMachine.TileItems[i, j]))
                    {
                        bonusesGrid.AddBonus(i, j, new TileItemIncomeBonus(effect.ModifierType, effect.ModificationValue));
                    }
                }
            }
        }

        private static void AddBonusToAdjacent(this SlotMachine slotMachine, TileItemBonusesGrid bonusesGrid, int i, int j, TileItemsEnhancingTileItemEffect effect)
        {
            var offsets = new[] { (-1, 0), (1, 0), (0, -1), (0, 1), (1, 1), (1, -1), (-1, 1), (-1, -1) };
            foreach (var (di, dj) in offsets)
            {
                int newI = i + di, newJ = j + dj;
                if (newI >= 0 && newI < slotMachine.TileItems.GetLength(0) && newJ >= 0 && newJ < slotMachine.TileItems.GetLength(1))
                {
                    var tileItem = slotMachine.TileItems[newI, newJ];
                    if (effect.Condition(tileItem))
                    {
                        bonusesGrid.AddBonus(newI, newJ, new TileItemIncomeBonus(effect.ModifierType, effect.ModificationValue));
                    }
                }
            }
        }

        private static void AddBonusToSquare(this SlotMachine slotMachine, TileItemBonusesGrid bonusesGrid, int i, int j, TileItemsEnhancingTileItemEffect effect, int size)
        {
            int halfSize = size / 2;
            for (int di = -halfSize; di <= halfSize; di++)
            {
                for (int dj = -halfSize; dj <= halfSize; dj++)
                {
                    int newI = i + di, newJ = j + dj;
                    if (newI >= 0 && newI < slotMachine.TileItems.GetLength(0) && newJ >= 0 && newJ < slotMachine.TileItems.GetLength(1))
                    {
                        var tileItem = slotMachine.TileItems[newI, newJ];
                        if (effect.Condition(tileItem))
                        {
                            bonusesGrid.AddBonus(newI, newJ, new TileItemIncomeBonus(effect.ModifierType, effect.ModificationValue));
                        }
                    }
                }
            }
        }

        private static void AddBonusToAllTiles(this SlotMachine slotMachine, TileItemBonusesGrid bonusesGrid, TileItemsEnhancingTileItemEffect effect)
        {
            for (int i = 0; i < slotMachine.TileItems.GetLength(0); i++)
            {
                for (int j = 0; j < slotMachine.TileItems.GetLength(1); j++)
                {
                    var tileItem = slotMachine.TileItems[i, j];
                    if (effect.Condition(tileItem))
                    {
                        bonusesGrid.AddBonus(i, j, new TileItemIncomeBonus(effect.ModifierType, effect.ModificationValue));
                    }
                }
            }
        }

    }

}
