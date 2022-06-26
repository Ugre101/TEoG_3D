using Items;
using UnityEngine;

namespace GameUIAndMenus.Menus.Inventory
{
    public class InventorySlot : MonoBehaviour
    {
        public delegate void MoveItems(InventoryItem item, Vector2 newPos);

        [SerializeField] InventorySlotItem slotItem;
        public Vector2 Position { get; private set; }

        public void Setup(Vector2 position) => Position = position;

        public void AddItem(InventoryItem invItem) => slotItem.Setup(invItem);

        public void ClearItem() => slotItem.Clear();

        public event MoveItems MovedItem;

        public void MoveTo(InventoryItem o) => MovedItem?.Invoke(o, Position);
    }
}