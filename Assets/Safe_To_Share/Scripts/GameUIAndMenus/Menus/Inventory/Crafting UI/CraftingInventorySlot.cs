using System;
using Items;
using UnityEngine.EventSystems;

namespace Safe_To_Share.Scripts.GameUIAndMenus.Menus.Inventory.Crafting_UI
{
    public class CraftingInventorySlot : InventorySlot,IPointerClickHandler
    {
        public event Action<InventoryItem> ReceivedItem;
        public event Action ClearedItem;
        public override void MoveTo(InventoryItem p, InventorySlot inventorySlot)
        {
            slotItem.Setup(p,this);
            ReceivedItem?.Invoke(p);
        }

        public override void ClearItem()
        {
            base.ClearItem();
            ClearedItem?.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            print("Clicked");
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                ClearItem();
            }
        }
    }
}