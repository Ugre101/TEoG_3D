using System;
using System.Collections.Generic;
using Items;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.Menus.Inventory
{
    public class InventoryBah : MonoBehaviour
    {
        [SerializeField] Transform content;
        [SerializeField] InventorySlot[] preInstancedSlots;
        [SerializeField] InventorySlot slot;
        bool firstUse = true;
        Dictionary<Vector2, InventorySlot> slots = new();
        Queue<InventorySlot> slotsPool;

        Items.Inventory inventory;

        public void Setup(Items.Inventory inv)
        {
            inventory = inv;
            if (firstUse)
            {
                firstUse = false;
                SetupSlotPool();
                // Reset
                slots = new Dictionary<Vector2, InventorySlot>();
                // Add slots
                for (int x = 0; x < inventory.InventorySize.x; x++)
                for (int y = 0; y < inventory.InventorySize.y; y++)
                    AddInventorySlot(x, y);
            }

            AddItems();
        }

#if UNITY_EDITOR
        void OnValidate() => preInstancedSlots = content.GetComponentsInChildren<InventorySlot>(true);
#endif

        InventorySlot GetSlot()
        {
            if (slotsPool.Count <= 0) return Instantiate(slot, content);
            var getSlot = slotsPool.Dequeue();
            getSlot.gameObject.SetActive(true);
            return getSlot;

        }


        void AddInventorySlot(int x, int y)
        {
            InventorySlot newSlot = GetSlot();
            newSlot.Setup(inventory,new Vector2(x, y));
            newSlot.MovedItem += MoveItem;
            slots.Add(newSlot.Position, newSlot);
        }

        void MoveItem(InventoryItem item, Vector2 newPos, InventorySlot oldSlot, InventorySlot newSlot)
        {
            if (inventory == oldSlot.belongsTo)
            {
                oldSlot.ClearItem();
                if (inventory.MoveItemInsideInventory(item, newPos, out var oldItem))
                {
                    AddInventoryItem(oldItem);
                    newSlot.ClearItem();
                }
                AddInventoryItem(item);
            }
            else
            {
                if (!inventory.HasSpace(item.ItemGuid, Vector2.one)) return;
                oldSlot.ClearItem();
                if (oldSlot.belongsTo.MoveToAnotherInventory(inventory,ref item, newPos, out var oldItem))
                {
                    oldSlot.AddItem(oldItem);
                    newSlot.ClearItem();
                }
                AddInventoryItem(item);
            }
        }

       public void AddItems()
        {
            foreach (var pair in slots)
                pair.Value.ClearItem();
            foreach (InventoryItem invItem in inventory.Items)
                AddInventoryItem(invItem);
        }

        void SetupSlotPool()
        {
            slotsPool = new Queue<InventorySlot>();
            foreach (var inventorySlot in preInstancedSlots)
            {
                inventorySlot.ClearItem();
                inventorySlot.gameObject.SetActive(false);
                slotsPool.Enqueue(inventorySlot);
            }
        }


        public void AddInventoryItem(InventoryItem invItem) => slots[invItem.Position].AddItem(invItem);

        public static event Action StopHoverInfo;

 

        void InventoryClearItemOnCord(Vector2 pos)
        {
            slots[pos].ClearItem();
            StopHoverInfo?.Invoke();
        }

        Items.Inventory secondaryInventory;
        public void SetupSecondaryInventory(Items.Inventory inventory)
        {
            secondaryInventory = inventory;
        }
    }
}