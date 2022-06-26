using System;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    [Serializable]
    public struct InventorySave
    {
        public List<SerializedItem> items;

        public InventorySave(IEnumerable<InventoryItem> inventoryItems)
        {
            items = new List<SerializedItem>();
            foreach (InventoryItem inventoryItem in inventoryItems)
                items.Add(new SerializedItem(inventoryItem));
        }

        [Serializable]
        public struct SerializedItem
        {
            public string guid;
            public int amount;
            public Vector2 pos;

            public SerializedItem(InventoryItem item)
            {
                guid = item.ItemGuid;
                amount = item.Amount;
                pos = item.Position;
            }
        }
    }
}