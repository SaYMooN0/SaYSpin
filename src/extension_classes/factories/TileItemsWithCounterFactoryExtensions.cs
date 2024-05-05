using SaYSpin.src.enums;
using SaYSpin.src.gameplay_parts.game_flow_controller;
using SaYSpin.src.inventory_items.tile_items;
using SaYSpin.src.inventory_items.tile_items.tile_item_effects;
using SaYSpin.src.secondary_classes;
using SaYSpin.src.tile_item_effects;

namespace SaYSpin.src.extension_classes.factories
{
    internal static class TileItemsWithCounterFactoryExtensions
    {
        public static TileItemWithCounter WithOnDestroyTileItemEffect(
            this TileItemWithCounter tileItem,
            string description,
            Action<GameFlowController> onDestroyAction) =>
                tileItem.WithEffect(new OnDestroyTileItemEffect(description, (game) => onDestroyAction(game)));

        public static TileItemWithCounter WithTileItemsEnhancingTileItemEffect(
            this TileItemWithCounter tileItem,
            string description,
            SlotMachineArea area,
            ModifierType modifierType,
            double modificationValue,
            Func<TileItem?, bool> condition) =>
                tileItem.WithEffect(new TileItemsEnhancingTileItemEffect(description, area, modifierType, modificationValue, condition));

        public static TileItemWithCounter WithAbsorbingTileItemEffect(
            this TileItemWithCounter tileItem,
            string description,
            Func<TileItem?, bool> absorbingCondition,
            Action<GameFlowController, TileItem> onAbsorbAction) =>
            tileItem.WithEffect(
                new AbsorbingTileItemEffect(
                    description,
                    absorbingCondition,
                    (game, ti) => onAbsorbAction(game, ti)
                    )
                );

        public static TileItemWithCounter WithAreaScanningTileItemEffect(this TileItemWithCounter tileItem,
            string description, SlotMachineArea area,
            Func<TileItem?, bool> condition,
            Action<GameFlowController, List<TileItemWithCoordinates>> onScanAction) =>
            tileItem.WithEffect(
                new AreaScanningTileItemEffect(description, area, condition, (game, scannedTileItems) => onScanAction(game, scannedTileItems))
            );
    }
}
