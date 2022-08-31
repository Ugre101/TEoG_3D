using Items;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.Menus.Inventory
{
    public class InventorySlot : MonoBehaviour
    {
        public delegate void MoveItems(InventoryItem item, Vector2 newPos,InventorySlot oldSlot,InventorySlot newSlot);

        [SerializeField] protected InventorySlotItem slotItem;
        public Vector2 Position { get; private set; }
        public Items.Inventory belongsTo { get; private set; }
        public void Setup(Items.Inventory inventory, Vector2 position)
        {
            belongsTo = inventory;
            Position = position;
            
        }

        public void AddItem(InventoryItem invItem)
        {
            slotItem.Setup(invItem, this);
        }

        public virtual void ClearItem()
        {
            slotItem.Clear();
        }

        public event MoveItems MovedItem;

        public virtual void MoveTo(InventoryItem p, InventorySlot inventorySlot) => MovedItem?.Invoke(p, Position,inventorySlot,this);

    }
}