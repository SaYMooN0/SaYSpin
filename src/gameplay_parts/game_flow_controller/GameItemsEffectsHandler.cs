using SaYSpin.src.enums;
using SaYSpin.src.extension_classes.slot_machine;
using SaYSpin.src.gameplay_parts.inventory_related.tokens;
using SaYSpin.src.inventory_items.relics;
using SaYSpin.src.inventory_items.relics.relic_effects;
using SaYSpin.src.inventory_items.tile_items;
using SaYSpin.src.inventory_items.tile_items.tile_item_effects;
using SaYSpin.src.secondary_classes;

namespace SaYSpin.src.gameplay_parts.game_flow_controller
{
    public partial class GameFlowController
    {
        public void HandleAfterSpinRelicEffects()
        {
            foreach (Relic r in Inventory.Relics)
            {
                foreach (AfterSpinRelicEffect rEffect in r.Effects.OfType<AfterSpinRelicEffect>())
                {
                    rEffect.PerformAfterSpinAction(this);
                }
            }
        }
        public void HandleTileItemsWithAreaScanningEffects()
        {
            for (int i = 0; i < SlotMachine.TileItems.GetLength(0); i++)
            {
                for (int j = 0; j < SlotMachine.TileItems.GetLength(1); j++)
                {
                    var tileItem = SlotMachine.TileItems[i, j];
                    if (tileItem is null) continue;

                    foreach (var effect in tileItem.Effects.OfType<AreaScanningTileItemEffect>())
                    {
                        var tileItemsInArea = SlotMachine.GetTileItemCoordinates(i, j, effect.Area, effect.Condition)
                            .Select(coords => new TileItemWithCoordinates(SlotMachine.TileItems[coords.row, coords.col], coords.row, coords.col))
                            .ToList();
                        effect.PerformOnScannedAction(this, tileItemsInArea);
                    }
                }
            }
        }

        public void HandleTransformationEffects()
        {
            List<Action> replacements = new();
            foreach (TileItem ti in Inventory.TileItems)
            {
                foreach (var effect in ti.Effects.OfType<TransformationTileItemEffect>())
                {
                    if (effect.TransformationCondition(this))
                    {
                        replacements.Add(() => ReplaceTileItem(ti, effect.TileItemToTransformInto));
                        break;
                    }
                }
            }
            foreach (Action replace in replacements)
                replace();
        }
        public void HandleTileItemsWithAbsorbingEffects()
        {
            for (int i = 0; i < SlotMachine.TileItems.GetLength(0); i++)
            {
                for (int j = 0; j < SlotMachine.TileItems.GetLength(1); j++)
                {
                    var item = SlotMachine.TileItems[i, j];
                    if (item is null) continue;

                    var absorbingEffects = item.Effects.OfType<AbsorbingTileItemEffect>();
                    foreach (var effect in absorbingEffects)
                    {
                        foreach (var (adjI, adjJ) in SlotMachine.GetTileItemCoordinates(i, j, SlotMachineArea.Adjacent, effect.AbsorbingCondition))
                        {
                            var adjItem = SlotMachine.TileItems[adjI, adjJ];
                            if (adjItem != null)
                            {
                                effect.ExecuteOnAbsorbAction(this, adjItem);
                                DestroyTileItem(adjItem, adjI, adjJ);
                            }
                        }
                    }
                }
            }
        }


        public IEnumerable<CoinsCalculationRelicEffect> GatherCoinsCalculationRelicEffects()
        {

            IEnumerable<CoinsCalculationRelicEffect> effects = Inventory.Relics
               .SelectMany(relic => relic.Effects.OfType<CoinsCalculationRelicEffect>());

            IEnumerable<CoinsCalculationRelicEffect> nonConstantEffects = Inventory.Relics
               .SelectMany(relic => relic.Effects.OfType<NonConstantCalculationRelicEffect>())
               .Select(eff => eff.GetCalculationEffect(this));

            return effects.Concat(nonConstantEffects);
        }
        public void TriggerOnNewStageChoosingSkippedEffects()
        {
            var effects = Inventory.Relics.SelectMany(r => r.Effects).OfType<OnNewStageChoosingSkippedRelicEffect>();
            foreach (var effect in effects)
            {
                effect.PerformOnNewStageChoosingSkippedAction(this);
            }
        }
        public void TriggerOnTokenUsedEffects(TokenType token)
        {
            var effects = Inventory.Relics.SelectMany(r => r.Effects).OfType<OnTokenUsedRelicEffect>();
            foreach (var effect in effects)
            {
                effect.PerformOnTokenUsedAction(this, token);
            }
        }
    }
}
