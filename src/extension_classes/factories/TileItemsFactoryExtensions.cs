using SaYSpin.src.enums;
using SaYSpin.src.inventory_items.tile_items;
using SaYSpin.src.inventory_items.tile_items.tile_item_effects;
using SaYSpin.src.tile_item_effects;
using static SaYSpin.src.inventory_items.tile_items.tile_item_effects.AbsorbingTileItemEffect;
using static SaYSpin.src.inventory_items.tile_items.tile_item_effects.OnDestroyTileItemEffect;

namespace SaYSpin.src.extension_classes.factories
{
    internal static class TileItemsFactoryExtensions
    {
        public static TileItem WithOnDestroyTileItemEffect(this TileItem tileItem, string description, OnDestroyAction onDestroyAction) =>
            tileItem.WithEffect(new OnDestroyTileItemEffect(description, onDestroyAction));
        public static TileItem WithTileItemsEnhancingTileItemEffect(this TileItem tileItem, string description, EffectApplicationArea area, ModifierType modifierType, double modificationValue, Func<TileItem, bool> condition) =>
            tileItem.WithEffect(new TileItemsEnhancingTileItemEffect(description, area, modifierType, modificationValue, condition));

        public static TileItem WithAbsorbingTileItemEffect(this TileItem tileItem, string description, Func<TileItem, bool> absorbingCondition, OnAbsorbActionDelegate onAbsorbAction) =>
            tileItem.WithEffect(new AbsorbingTileItemEffect(description, absorbingCondition, onAbsorbAction));
    }
}
