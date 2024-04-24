using SaYSpin.src.enums;
using SaYSpin.src.gameplay_parts;
using SaYSpin.src.inventory_items.tile_items;
using SaYSpin.src.inventory_items.tile_items.tile_item_effects;
using SaYSpin.src.tile_item_effects;

namespace SaYSpin.src.extension_classes.factories
{
    internal static class TileItemsWithCounterFactoryExtensions
    {
        public static TileItemWithCounter WithOnDestroyTileItemEffect(
            this TileItemWithCounter tileItem,
            string description,
            Action<GameFlowController, TileItemWithCounter> onDestroyAction) =>
                tileItem.WithEffect(new OnDestroyTileItemEffect(description, (game) => onDestroyAction(game, tileItem)));

        public static TileItemWithCounter WithTileItemsEnhancingTileItemEffect(
            this TileItemWithCounter tileItem,
            string description,
            SlotMachineArea area,
            ModifierType modifierType,
            Func<TileItemWithCounter, double> modificationValue,
            Func<TileItem, bool> condition) =>
                tileItem.WithEffect(new TileItemsEnhancingTileItemEffect(description, area, modifierType, modificationValue(tileItem), condition));

        public static TileItemWithCounter WithAbsorbingTileItemEffect(
            this TileItemWithCounter tileItem,
            string description,
            Func<TileItem, bool> absorbingCondition,
            Action<GameFlowController, TileItemWithCounter, TileItem> onAbsorbAction) =>
            tileItem.WithEffect(
                new AbsorbingTileItemEffect(
                    description,
                    absorbingCondition,
                    (game, absorbedtTI) => onAbsorbAction(game, tileItem, absorbedtTI)
                    )
                );
    }
}
