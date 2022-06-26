using System;
using System.Collections.Generic;
using Character.PlayerStuff;
using Character.Shop;
using Items;
using UnityEngine;

namespace Character.Npc
{
    [System.Serializable]
    public class TradeAbleCharacter: BaseNpc
    {
        public static event Action<string, List<Item>> OpenShopMenu;
        [SerializeField] string shopTitle;
        [SerializeField] ShopItems itemOnSales;
        public override string HoverText(Player player) => "Trade";
        public override void DoInteraction(Player player)
        {
            if (itemOnSales == null)
                return;
            OpenShopMenu?.Invoke(shopTitle, itemOnSales.SellingItems);
        }
    }
}