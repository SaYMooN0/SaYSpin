using SaYSpin.src.inventory_items;
using SaYSpin.src.inventory_items.relics;
using SaYSpin.src.inventory_items.relics.relic_effects;
using SaYSpin.src.inventory_items.tile_items;
using SaYSpin.src.inventory_items.tile_items.tile_item_effects;

namespace SaYSpin.src.gameplay_parts.game_flow_controller
{
    public partial class GameFlowController
    {
        internal void DestroyTileItem(TileItem tileItemToDestroy, int row, int col)
        {
            OnTileItemDestruction?.Invoke(tileItemToDestroy);
            foreach (OnDestroyTileItemEffect effect in tileItemToDestroy.Effects.OfType<OnDestroyTileItemEffect>())
            {
                effect.PerformOnDestroyAction(this);
            }
            Inventory.TileItems.Remove(tileItemToDestroy);
            SlotMachine.SetTileItemNull(row, col);
        }
        internal void AddBaseInventoryItemToInventory(BaseInventoryItem item)
        {
            if (item is Relic relic)
                AddRelicToInventory(relic);
            else if (item is TileItem tileItem)
                AddTileItemToInventory(tileItem);
            else   throw new System.ArgumentException("Incorrect item type");
        }
        internal void AddTileItemToInventory(TileItem item)
        {
            foreach (var effect in Inventory.Relics.SelectMany(r => r.Effects.OfType<TileItemAddingInterceptionRelicEffect>()))
            {
                if (effect.InterceptionCondition(item))
                {
                    item = effect.InterceptionFunc(this, item);
                    break;
                }
            }
            if (item is null)
                return;
            TileItem itemToAdd = allTileItemsConstructors[item.Name]();

            Inventory.TileItems.Add(itemToAdd);
            OnInventoryItemAdded?.Invoke(itemToAdd);
        }
        internal void AddRelicToInventory(Relic relic)
        {
            Relic relicToAdd = allRelicsConstructors[relic.Name]();

            Inventory.Relics.Add(relicToAdd);
            OnInventoryItemAdded?.Invoke(relicToAdd);

            if (relicToAdd.Effects.Any(e => e is GameStatRelicEffect))
                StatsTracker.SetChanged();
        }
        internal bool RemoveTileItemFromInventory(TileItem tileItem)
        {
            if (!Inventory.TileItems.Contains(tileItem))
                return false;
            Inventory.TileItems.Remove(tileItem);
            OnInventoryItemRemoved?.Invoke(tileItem);
            return true;
        }
        internal bool RemoveRelicFromInventory(Relic relic)
        {
            if (!Inventory.Relics.Contains(relic))
                return false;

            if (relic.Effects.Any(e => e is GameStatRelicEffect))
                StatsTracker.SetChanged();

            Inventory.Relics.Remove(relic);
            OnInventoryItemRemoved?.Invoke(relic);

            return true;
        }
        internal bool RemoveTileItemFromInventoryById(string tileItemId)
        {
            TileItem item = Inventory.TileItems.FirstOrDefault(ti => ti.Id == tileItemId);
            if (item == null)
                return false;

            return RemoveTileItemFromInventory(item);
        }
        internal bool RemoveRelicFromInventoryById(string relicId)
        {
            Relic relic = Inventory.Relics.FirstOrDefault(r => r.Id == relicId);
            if (relic == null)
                return false;

            return RemoveRelicFromInventory(relic);
        }
        internal void ReplaceTileItem(TileItem oldTileItem, TileItem newTileItem)
        {
            int index = Inventory.TileItems.IndexOf(oldTileItem);
            if (newTileItem is null)
                Inventory.TileItems.RemoveAt(index);
            else
                Inventory.TileItems[index] = newTileItem;
        }
    }
}
