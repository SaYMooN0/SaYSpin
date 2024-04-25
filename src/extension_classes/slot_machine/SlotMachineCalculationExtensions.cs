using SaYSpin.src.enums;
using SaYSpin.src.extension_classes.slot_machine;
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
                        int income = item.CalculateIncome(bonuses.GetBonusesFor(i, j));
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
            foreach (var (newI, newJ) in slotMachine.GetTileItemCoordinates(i, j, effect.Area, effect.Condition))
            {
                bonusesGrid.AddBonus(newI, newJ, new TileItemIncomeBonus(effect.ModifierType, effect.ModificationValue));
            }
        }

    }

}
