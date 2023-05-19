using Items;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.ShopMenu
{
    public sealed class SellItemBag : MonoBehaviour
    {
        [SerializeField] SellMyItem sellMyItem;
        [SerializeField] Transform content;

        public void Setup(Inventory inventory)
        {
            content.KillChildren();
            foreach (InventoryItem inventoryItem in inventory.Items)
                Instantiate(sellMyItem, content).Setup(inventoryItem);
        }
    }
}