using System;
using Items;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.GameUIAndMenus.ShopMenu
{
    public sealed class SellMyItem : MonoBehaviour
    {
        [SerializeField] Image icon;
        [SerializeField] Button btn;
        InventoryItem item;
        public static event Action<InventoryItem> SellMe;

        public void Setup(InventoryItem sellMe)
        {
            item = sellMe;
            // icon.sprite = sellMe.ItemGuid.Icon;
            btn.onClick.AddListener(Sell);
        }

        void Sell() => SellMe?.Invoke(item);
    }
}