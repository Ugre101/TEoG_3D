namespace GameUIAndMenus.Menus.Inventory
{
    public class InventoryHoverText : ItemBaseHoverText
    {
        bool started;

        void Start()
        {
            InventorySlotItem.ShowItem += ShowItem;
            InventorySlotItem.StopShowing += StopShowing;
            InventoryMenu.StopHoverInfo += StopShowing;
            gameObject.SetActive(false);
            started = true;
        }

        void OnDisable()
        {
            if (started)
                gameObject.SetActive(false);
        }

        void OnDestroy()
        {
            InventorySlotItem.ShowItem -= ShowItem;
            InventorySlotItem.StopShowing -= StopShowing;
            InventoryMenu.StopHoverInfo -= StopShowing;
        }
    }
}