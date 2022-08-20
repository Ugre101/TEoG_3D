using Items;
using UnityEngine;

namespace GameUIAndMenus.Menus.Inventory
{
    public class InventorySlot : MonoBehaviour
    {
        public delegate void MoveItems(Items.Inventory from, InventoryItem item, Vector2 newPos,InventorySlot oldSlot,InventorySlot newSlot);

        [SerializeField] InventorySlotItem slotItem;
        public Vector2 Position { get; private set; }
        Items.Inventory belongsTo;
        public void Setup(Items.Inventory inventory, Vector2 position)
        {
            belongsTo = inventory;
            Position = position;
        }

        public void AddItem(InventoryItem invItem) => slotItem.Setup(invItem,this);

        public void ClearItem() => slotItem.Clear();

        public event MoveItems MovedItem;

        public void MoveTo(InventoryItem p, InventorySlot inventorySlot) => MovedItem?.Invoke(belongsTo, p, Position,inventorySlot,this);
    }
}