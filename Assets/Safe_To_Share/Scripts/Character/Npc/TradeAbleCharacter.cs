using System;
using System.Collections.Generic;
using Character.PlayerStuff;
using Character.Shop;
using Items;
using UnityEngine;

namespace Character.Npc {
    [Serializable]
    public class TradeAbleCharacter : BaseNpc {
        [SerializeField] string shopTitle;
        [SerializeField] ShopItems itemOnSales;
        public static event Action<string, List<Item>> OpenShopMenu;
        public override string HoverText(Player player) => "Trade";

        public override void DoInteraction(Player player) {
            if (itemOnSales == null)
                return;
            OpenShopMenu?.Invoke(shopTitle, itemOnSales.SellingItems);
        }
    }
}