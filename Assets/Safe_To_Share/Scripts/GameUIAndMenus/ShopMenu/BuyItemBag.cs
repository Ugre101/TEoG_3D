using System.Collections.Generic;
using Items;
using UnityEngine;

namespace Shop.UI
{
    public class BuyItemBag : MonoBehaviour
    {
        [SerializeField] ItemForSale prefab;
        [SerializeField] Transform content;

        public void Setup(List<Item> shopItems)
        {
            content.KillChildren();
            foreach (Item sellingItem in shopItems) 
                Instantiate(prefab, content).Setup(sellingItem);
        }
    }
}