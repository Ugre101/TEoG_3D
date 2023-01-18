using Character.PlayerStuff;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.GameUIAndMenus
{
    public class AmountOfItem : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI amountText;
        [SerializeField] Image itemImage;

        public void Setup(Player player, Item opResult)
        {
            itemImage.sprite = opResult.Icon;
            if (player.Inventory.TryGetItemByGuid(opResult.Guid, out var itemInInventory))
            {
                itemImage.color = Color.white;
                amountText.text = itemInInventory.Amount.ToString();
                itemInInventory.AmountChange += i => amountText.text = i.ToString();
            }
            else
            {
                amountText.text = "0";
                itemImage.color = Color.gray;
            }
        }
    }
}