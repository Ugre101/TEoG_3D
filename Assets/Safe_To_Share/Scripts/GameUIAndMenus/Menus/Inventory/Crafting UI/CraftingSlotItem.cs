using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Safe_To_Share.Scripts.GameUIAndMenus.Menus.Inventory.Crafting_UI
{
    public sealed class CraftingSlotItem : InventorySlotItem
    {
        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            List<RaycastResult> results = new();
            EventSystem.current.RaycastAll(eventData, results);
            foreach (RaycastResult o in results)
            {
                if (!o.gameObject.TryGetComponent(out CraftingInventorySlot newSlot)) continue;
                ResetPosition();
                return;
            }
            ResetPosition();
            slot.ClearItem();
        }
    }
}