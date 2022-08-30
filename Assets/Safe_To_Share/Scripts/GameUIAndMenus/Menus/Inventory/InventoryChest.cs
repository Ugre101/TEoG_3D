using System;
using System.Collections;
using Character.PlayerStuff;
using CustomClasses;
using Items;
using Safe_To_Share.Scripts.Character.Items;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.Menus.Inventory
{
    public class InventoryChest : MonoBehaviour,IInteractable
    {

        [SerializeField] ChestItems[] items;
        
        [SerializeField,HideInInspector] string guid;
        Items.Inventory inventory = new();

        public static event Action<Items.Inventory> OpenInventory; 
        IEnumerator GetInventory()
        {
            if (WorldInventories.Inventories.TryGetValue(guid, out var myInventory))
                inventory = myInventory;
            else
                yield return NewInventory();
            OpenInventory?.Invoke(inventory);
        }

        IEnumerator NewInventory()
        {
            if (!WorldInventories.Inventories.TryAdd(guid, inventory))
                throw new Exception("Couldn't find or add world inventory");
            foreach (var toAdd in items)
            {
                yield return inventory.AddItemWithGuid(toAdd.Item.guid, toAdd.Amount);
            }
        }


#if UNITY_EDITOR
        void OnValidate()
        {
            if (string.IsNullOrEmpty(guid))
                guid = Guid.NewGuid().ToString("N");
        }
#endif
        public string HoverText(Player player) => "Open";

        public void DoInteraction(Player player)
        {
            StartCoroutine(GetInventory());
        }

        public event Action<IInteractable> UpdateHoverText;
        public event Action RemoveIInteractableHit;
        [Serializable]
        struct ChestItems
        {
            [field: SerializeField] public DropSerializableObject<Item> Item { get; private set; }
            [field: SerializeField] public int Amount { get; private set; }
        }
    }
}