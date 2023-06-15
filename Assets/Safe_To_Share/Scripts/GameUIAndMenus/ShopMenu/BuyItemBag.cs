using System.Collections.Generic;
using Items;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.ShopMenu {
    public sealed class BuyItemBag : MonoBehaviour {
        [SerializeField] ItemForSale prefab;
        [SerializeField] Transform content;

        public void Setup(List<Item> shopItems) {
            content.KillChildren();
            foreach (var sellingItem in shopItems)
                Instantiate(prefab, content).Setup(sellingItem);
        }
    }
}