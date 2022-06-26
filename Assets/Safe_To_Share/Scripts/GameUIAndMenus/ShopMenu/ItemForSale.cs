using System;
using Items;
using UnityEngine;
using UnityEngine.UI;

namespace Shop.UI
{
    public class ItemForSale : MonoBehaviour
    {
        [SerializeField] Image icon;
        [SerializeField] Color afford, cantAfford;
        [SerializeField] Button btn;

        Item item;

        void OnDisable() => ShopMenu.CanAfford -= CanAfford;

        public static event Action<Item> WantToBuyItem;


        public void Setup(Item newItem)
        {
            item = newItem;
            icon.sprite = newItem.Icon;
            //icon.color = gold.CanAfford(newItem.Value) ? afford : cantAfford;
            btn.onClick.AddListener(Buy);
            ShopMenu.CanAfford += CanAfford;
        }

        void CanAfford(int obj) => icon.color = item.Value <= obj ? afford : cantAfford;

        void Buy() => WantToBuyItem?.Invoke(item);
    }
}