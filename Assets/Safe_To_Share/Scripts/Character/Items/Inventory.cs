using System;
using System.Collections;
using System.Collections.Generic;
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
            var item = GetItemByID(guid);
            return item != null && item.Amount >= amountNeeded;
        }

        public void UseItemItemID(string id, int times = 1) => LoadAndUseItem(GetItemByID(id), times);
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

        public IEnumerator AddItemWithGuid(string itemGuid, int quantity = 1)
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

        bool ItemExists(Vector2 pos) => Items.Exists(i => ItemExistOnPos(i, pos));
        static bool ItemExistOnPos(InventoryItem i, Vector2 pos) => i.Position == pos;
        InventoryItem GetItemOnPos(Vector2 pos) => Items.Find(i => ItemExistOnPos(i, pos));
        InventoryItem GetItemByID(string id) => Items.Find(i => i.ItemGuid == id);

        bool TryGetItemOnPos(Vector2 pos, out InventoryItem item)
        {
            item = null;
            if (!ItemExists(pos))
                return false;
            item = GetItemOnPos(pos);
            return true;
        }

        public bool MoveItem(InventoryItem toMove, Vector2 newPos, out InventoryItem oldItem)
        {
            bool hadItem = TryGetItemOnPos(newPos, out oldItem);
            if (hadItem)
                oldItem.Position = toMove.Position;
            toMove.Position = newPos;
            return hadItem;
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