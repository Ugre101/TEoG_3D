using System;
using System.Collections;
using Character.PlayerStuff;
using CustomClasses;
using Items;
using Safe_To_Share.Scripts.Character.Items;
using UnityEngine;

namespace Safe_To_Share.Scripts.Building
{
    public class InventoryChest : MonoBehaviour,IInteractable
    {

        [SerializeField] DropSerializableObject<Item>[] items;
        
        [SerializeField,HideInInspector] string guid;
        Inventory inventory = new();

        public static event Action<Inventory> OpenInventory; 
        IEnumerator Start()
        {
            if (WorldInventories.Inventories.TryGetValue(guid, out var myInventory))
                inventory = myInventory;
            else
            {
                if (!WorldInventories.Inventories.TryAdd(guid, inventory))
                    throw new Exception("Couldn't find or add world inventory");
                foreach (var toAdd in items)
                {
                   yield return inventory.AddItemWithGuid(toAdd.guid);
                }
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
            OpenInventory?.Invoke(inventory);
        }

        public event Action<IInteractable> UpdateHoverText;
        public event Action RemoveIInteractableHit;
    }
}