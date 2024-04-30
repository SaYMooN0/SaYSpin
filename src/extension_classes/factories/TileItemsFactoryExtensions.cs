using SaYSpin.src.enums;
using SaYSpin.src.gameplay_parts.game_flow_controller;
using SaYSpin.src.inventory_items.tile_items;
using SaYSpin.src.inventory_items.tile_items.tile_item_effects;
using SaYSpin.src.secondary_classes;
using SaYSpin.src.tile_item_effects;
using static SaYSpin.src.inventory_items.tile_items.tile_item_effects.AbsorbingTileItemEffect;
using static SaYSpin.src.inventory_items.tile_items.tile_item_effects.AreaScanningTileItemEffect;

namespace SaYSpin.src.extension_classes.factories
{
    internal static class TileItemsFactoryExtensions
    {
        public static TileItem WithOnDestroyTileItemEffect(this TileItem tileItem, string description, Action<GameFlowController> onDestroyAction) =>
            tileItem.WithEffect(new OnDestroyTileItemEffect(description, tileItem, onDestroyAction));
        public static TileItem WithTileItemsEnhancingTileItemEffect(this TileItem tileItem, string description, SlotMachineArea area, ModifierType modifierType, double modificationValue, Func<TileItem?, bool> condition) =>
            tileItem.WithEffect(new TileItemsEnhancingTileItemEffect(description, tileItem, area, modifierType, modificationValue, condition));
        public static TileItem WithAbsorbingTileItemEffect(this TileItem tileItem, string description, Func<TileItem?, bool> absorbingCondition, OnAbsorbActionDelegate onAbsorbAction) =>
            tileItem.WithEffect(new AbsorbingTileItemEffect(description, tileItem, absorbingCondition, onAbsorbAction));
        public static TileItem WithTransformationEffect(this TileItem tileItem, string description, Func<GameFlowController, bool> condition, TileItem tileItemToTransformInto) =>
            tileItem.WithEffect(new TransformationTileItemEffect(description, tileItem, condition, tileItemToTransformInto));
        public static TileItem WithAreaScanningTileItemEffect(this TileItem tileItem,
            string description, SlotMachineArea area,
            Func<TileItem?, bool> condition, AreaScanningDelegate onScanAction) =>
            tileItem.WithEffect(new AreaScanningTileItemEffect(description, tileItem, area, condition, onScanAction));

    }
}
