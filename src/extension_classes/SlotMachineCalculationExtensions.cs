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
                    if (item != null)
                    {
                        coinValue += item.CalculateIncome(
                            bonuses.GetBonusesFor(i, j));
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
                case EffectApplicationArea.Self:
                    if (effect.Condition(slotMachine.TileItems[i, j]))
                    {
                        bonusesGrid.AddBonus(i, j, new TileItemIncomeBonus(effect.ModifierType, effect.ModificationValue));
                    }
                    break;
                case EffectApplicationArea.Adjacent:
                    slotMachine.AddBonusToAdjacent(bonusesGrid, i, j, effect);
                    break;
                case EffectApplicationArea.Square3:
                case EffectApplicationArea.Square5:
                    slotMachine.AddBonusToSquare(bonusesGrid, i, j, effect, effect.Area == EffectApplicationArea.Square3 ? 3 : 5);
                    break;
                case EffectApplicationArea.AllTiles:
                    slotMachine.AddBonusToAllTiles(bonusesGrid, effect);
                    break;
                default:
                    throw new NotImplementedException($"EffectApplicationArea {effect.Area} is not implemented.");
            }
        }

        private static void AddBonusToAdjacent(this SlotMachine slotMachine, TileItemBonusesGrid bonusesGrid, int i, int j, TileItemsEnhancingTileItemEffect effect)
        {
            var offsets = new[] { (-1, 0), (1, 0), (0, -1), (0, 1) , (1,1), (1, -1), (-1, 1), (-1, -1) };
            foreach (var (di, dj) in offsets)
            {
                int newI = i + di, newJ = j + dj;
                if (newI >= 0 && newI < slotMachine.TileItems.GetLength(0) && newJ >= 0 && newJ < slotMachine.TileItems.GetLength(1))
                {
                    var tileItem = slotMachine.TileItems[newI, newJ];
                    if (tileItem is not null && effect.Condition(tileItem))
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
                        if (tileItem is not null && effect.Condition(tileItem))
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
                    if (tileItem is not null && effect.Condition(tileItem))
                    {
                        bonusesGrid.AddBonus(i, j, new TileItemIncomeBonus(effect.ModifierType, effect.ModificationValue));
                    }
                }
            }
        }

    }

}
