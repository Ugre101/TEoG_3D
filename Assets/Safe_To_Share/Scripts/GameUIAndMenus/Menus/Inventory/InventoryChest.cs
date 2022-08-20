using System;
using System.Collections.Generic;
using Character.PlayerStuff;
using Items;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Safe_To_Share.Scripts.Building
{
    public class InventoryChest : MonoBehaviour,IInteractable
    {
        [SerializeField] string guid;
        Inventory inventory = new();

        public static event Action<Inventory> OpenInventory; 
        void Start()
        {
            if (WorldInventories.Inventories.TryGetValue(guid, out var myInventory))
                inventory = myInventory;
            else
            {
                if (!WorldInventories.Inventories.TryAdd(guid, inventory))
                    throw new Exception("Couldn't find or add world inventory");
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
    public static class WorldInventories
    {
        public static Dictionary<string, Inventory> Inventories = new();

        public static void Save()
        {
            
        }

        public static void Load()
        {
            
        }
    }
}