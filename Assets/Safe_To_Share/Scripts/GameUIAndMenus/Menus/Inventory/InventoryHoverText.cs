namespace Safe_To_Share.Scripts.GameUIAndMenus.Menus.Inventory {
    public sealed class InventoryHoverText : ItemBaseHoverText {
        bool started;

        void Start() {
            InventorySlotItem.ShowItem += ShowItem;
            InventorySlotItem.StopShowing += StopShowing;
            InventoryMenu.StopHoverInfo += StopShowing;
            InventoryMenu2.StopHoverInfo += StopShowing;
            gameObject.SetActive(false);
            started = true;
        }

        void OnDisable() {
            if (started)
                gameObject.SetActive(false);
        }

        void OnDestroy() {
            InventorySlotItem.ShowItem -= ShowItem;
            InventorySlotItem.StopShowing -= StopShowing;
            InventoryMenu.StopHoverInfo -= StopShowing;
            InventoryMenu2.StopHoverInfo -= StopShowing;
        }
    }
}