using System;
using Items;

namespace Safe_To_Share.Scripts.GameUIAndMenus.Menus.Inventory.Crafting_UI
{
    public sealed class CraftingInventorySlot : InventorySlot
    {
        public event Action<InventoryItem> ReceivedItem;
        public event Action ClearedItem;
        InventorySlot orgSlot;

        public override void MoveTo(InventoryItem p, InventorySlot inventorySlot)
        {
            orgSlot = inventorySlot;
            slotItem.Setup(p, this);
            ReceivedItem?.Invoke(p);
        }

        public void ClearOrgItemThenClear()
        {
            orgSlot.ClearItem();
            base.ClearItem();
            ClearedItem?.Invoke();
        }
        public override void ClearItem()
        {
            base.ClearItem();
            ClearedItem?.Invoke();
        }
    }
}