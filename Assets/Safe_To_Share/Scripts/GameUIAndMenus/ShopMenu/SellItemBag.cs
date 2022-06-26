using Items;
using UnityEngine;

namespace Shop.UI
{
    public class SellItemBag : MonoBehaviour
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