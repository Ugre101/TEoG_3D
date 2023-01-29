using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Character;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Items
{
    [Serializable]
    public class Inventory
    {
        [SerializeField] List<InventoryItem> items = new();

        public List<InventoryItem> Items
        {
            get => items;
            private set => items = value;
        }

        public Vector2 InventorySize { get; } = new(7, 4);

        void LoadAndUseItem(InventoryItem item, int times = 1) =>
            Addressables.LoadAssetAsync<Item>(item.ItemGuid).Completed += i => UseAfterLoad(i, item, times);

        void UseAfterLoad(AsyncOperationHandle<Item> asyncOperationHandle, InventoryItem item, int times)
        {
            if (!asyncOperationHandle.Result.UnlimitedUse)
                item.Amount -= times;
            if (item.Amount >= 1)
                return;
            Items.Remove(item);
        }

        public bool HasItemOfGuid(string guid, int amountNeeded = 1)
        {
            var item = GetItemByGuid(guid);
            return item != null && item.Amount >= amountNeeded;
        }

        public int GetAmountOfItem(string guid)
        {
            var item = GetItemByGuid(guid);
            return item?.Amount ?? 0;
        }
        public void UseItemItemID(string id, int times = 1) => LoadAndUseItem(GetItemByGuid(id), times);

        public async Task<bool> LoadAndUseItemId(string guid, BaseCharacter user, int times = 1)
        {
            var invenItem = GetItemByGuid(guid);
            if (invenItem == null)
                return false;
            var task = Addressables.LoadAssetAsync<Item>(invenItem.ItemGuid);
            await task.Task;
            if (task.Status == AsyncOperationStatus.Succeeded)
            {
                Item item = task.Result;
                item.Use(user);
                if (!item.UnlimitedUse)
                    invenItem.Amount -= times;
                if (invenItem.Amount >= 1)
                    return false;
                Items.Remove(invenItem);
            }

            return false;
            // .Completed += i => UseAfterLoad(i, item, times);
        }
        public bool LowerItemAmountWithoutLoading(string guid, int amount = 1)
        {
            var item = GetItemByGuid(guid);
            if (item == null)
                return false;
            item.Amount -= amount;
            if (item.Amount <= 0)
                Items.Remove(item);
            return true;
        }
        public async Task<bool> LowerItemAmount(string itemGuid, int amount = 1)
        {
            if (itemGuid == null) return false;
            var task = Addressables.LoadAssetAsync<Item>(itemGuid);
            await task.Task;
            if (task.Status != AsyncOperationStatus.Succeeded) throw new Exception("Failed to load item");
            if (task.Result.UnlimitedUse) return true;
            var invItem = Items.FindAll(i => i.ItemGuid == itemGuid);
            if (invItem.Count <= 0)
                return false;
            int tot = invItem.Sum(i => i.Amount);
            if (tot < amount)
                return false;

            int toRemove = amount;
            foreach (var inventoryItem in invItem)
            {
                if (inventoryItem.Amount <= toRemove)
                {
                    toRemove -= inventoryItem.Amount;
                    items.Remove(inventoryItem);
                }
                else
                {
                    inventoryItem.Amount -= toRemove;
                }
            }
            return true;

        }

        public async Task<bool> LowerItemAmountAndReturnIfStillHave(string itemGuid, int amount = 1)
        {
            var op = await LowerItemAmount(itemGuid, amount);
            if (op)
            {
                if (!HasItemOfGuid(itemGuid)) return false;
                return GetItemSymByID(itemGuid) > 0;
            }

            return false;
        }

        public void UseItemOnCords(Vector2 pos, int times = 1) => LoadAndUseItem(GetItemOnPos(pos), times);

        public bool UseLoadedItem(Item item, Vector2 pos, int times = 1)
        {
            InventoryItem itemOnPos = GetItemOnPos(pos);
            if (itemOnPos == null)
                throw new ArgumentNullException(nameof(itemOnPos), "InvItem is null");
            if (!item.UnlimitedUse)
                itemOnPos.Amount -= times;
            if (itemOnPos.Amount >= 1)
                return false;
            Items.Remove(itemOnPos);
            return true;
        }

        public IEnumerator LoadAndAddItemWithGuid(string itemGuid, int quantity = 1)
        {
            var op = Addressables.LoadAssetAsync<Item>(itemGuid);
            yield return op;
            if (op.Status == AsyncOperationStatus.Succeeded) AddItem(op.Result, quantity);
        }
        public bool AddItem(Item item, int quantity = 1) => AddItem(item.Guid, quantity);

        public bool AddItem(string itemGuid, int quantity = 1)
        {
            if (Items.Exists(i => i.ItemGuid == itemGuid))
            {
                Items.Find(i => i.ItemGuid == itemGuid).Amount += quantity;
                return true;
            }

            for (int x = 0; x < InventorySize.x; x++)
            for (int y = 0; y < InventorySize.y; y++)
            {
                Vector2 pos = new(x, y);
                if (ItemExists(pos)) continue;
                Items.Add(new InventoryItem(itemGuid, quantity, pos));
                return true;
            }

            return false; // inventory full
        }        
        
        public bool AddAndReturnIfNewItem(string itemGuid,out InventoryItem newItem, int quantity = 1)
        {
            newItem = null;
            if (Items.Exists(i => i.ItemGuid == itemGuid))
            {
                Items.Find(i => i.ItemGuid == itemGuid).Amount += quantity;
                return false;
            }

            for (int x = 0; x < InventorySize.x; x++)
            for (int y = 0; y < InventorySize.y; y++)
            {
                Vector2 pos = new(x, y);
                if (ItemExists(pos)) continue;
                newItem = new InventoryItem(itemGuid, quantity, pos);
                Items.Add(newItem);
                return true;
            }

            return false; // inventory full
        }

        public bool HasSpace(string itemGuid, Vector2 size)
        {
            if (Items.Exists(i => i.ItemGuid == itemGuid))
            {
                return true; // Can stack
            }
            for (int x = 0; x < InventorySize.x; x++)
            for (int y = 0; y < InventorySize.y; y++)
            {
                Vector2 pos = new(x, y);
                if (ItemExists(pos)) continue;
                // TODO check if larger than 1x1 fits
                return true;
            }

            return false;
        }

        bool ItemExists(Vector2 pos) => Items.Exists(i => ItemExistOnPos(i, pos));
        static bool ItemExistOnPos(InventoryItem i, Vector2 pos) => i.Position == pos;
        InventoryItem GetItemOnPos(Vector2 pos) => Items.Find(i => ItemExistOnPos(i, pos));
        InventoryItem GetItemByGuid(string id) => Items.Find(i => i.ItemGuid == id);

        int GetItemSymByID(string id)
        {
           var finds =  Items.FindAll(i => i.ItemGuid == id);
           if (finds.Count <= 0) return 0;
           return finds.Sum(i => i.Amount);
        }
        public bool TryGetItemOnPos(Vector2 pos, out InventoryItem item)
        {
            item = null;
            if (!ItemExists(pos))
                return false;
            item = GetItemOnPos(pos);
            return true;
        }
        public bool TryGetItemByGuid(string guid, out InventoryItem item)
        {
            item = null;
            if (!HasItemOfGuid(guid))
                return false;
            item = GetItemByGuid(guid);
            return true;
        }
        public bool MoveItemInsideInventory(InventoryItem toMove, Vector2 newPos, out InventoryItem oldItem)
        {
            bool hadItem = TryGetItemOnPos(newPos, out oldItem);
            if (hadItem)
                oldItem.Position = toMove.Position;
            toMove.Position = newPos;
            return hadItem;
        }

        public bool AddInventoryItemToPos(ref InventoryItem toAdd, Vector2 toPos, out InventoryItem oldItem)
        {
            bool hadItem = TryGetItemOnPos(toPos, out oldItem);
            if (hadItem && oldItem.ItemGuid == toAdd.ItemGuid)
            {
                oldItem.Amount += toAdd.Amount;
                toAdd = oldItem;
                return false; 
            }
            toAdd.Position = toPos;
            Items.Add(toAdd);
            return hadItem;
        }

        public bool MoveToAnotherInventory(Inventory moveTo,ref InventoryItem toMove,Vector2 toPos,out InventoryItem oldItem)
        {
            oldItem = null;
            Vector2 oldPos = toMove.Position;
            Items.Remove(toMove);
            if (moveTo.AddInventoryItemToPos(ref toMove, toPos, out var item))
            {
                oldItem = item;
                oldItem.Position = oldPos;
                Items.Add(oldItem);
                return true;
            }

            return false;
        }

        public InventorySave Save() => new(Items);

        public void Load(InventorySave toLoad)
        {
            Items = new List<InventoryItem>();
            foreach (InventorySave.SerializedItem loadItem in toLoad.items)
                Items.Add(new InventoryItem(loadItem.guid, loadItem.amount, loadItem.pos));
        }


        
    }
}