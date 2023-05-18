using System;
using System.Collections.Generic;
using Character.PlayerStuff.Currency;
using Currency;
using Currency.UI;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.GameUIAndMenus.ShopMenu
{
    public class ShopMenu : GameMenu
    {
        [SerializeField] TextMeshProUGUI shopTitleText;
        [SerializeField] BuyItemBag buyItemBag;
        [SerializeField] Button toggleSell;
        [SerializeField] TextMeshProUGUI toggleSellText;
        [SerializeField] HaveGold haveGold;
        [SerializeField] ItemShowCase buyItemShowCase;
        bool selling;
        List<Item> shopItems;

        void Start()
        {
            // toggleSell.onClick.AddListener(ToggleSelling);
        }

        void OnEnable()
        {
            ItemForSale.WantToBuyItem += GetItem;
            SellMyItem.SellMe += HandleSoldItem;
            ItemShowCase.BuyItemInAmount += BuyItem;
        }

        void OnDisable()
        {
            ItemForSale.WantToBuyItem -= GetItem;
            SellMyItem.SellMe -= HandleSoldItem;
            ItemShowCase.BuyItemInAmount -= BuyItem;
            PlayerGold.GoldBag.GoldAmountChanged -= GoldChanged;
        }

        void BuyItem(Item arg1, int arg2)
        {
            if (Player.TryBuyItem(arg1, arg2))
                buyItemShowCase.Setup(arg1, PlayerGold.GoldBag);
        }

        public static event Action<int> CanAfford;

        void HandleSoldItem(InventoryItem obj)
        {
        }

        void GetItem(Item obj)
        {
            buyItemShowCase.gameObject.SetActive(true);
            buyItemShowCase.Setup(obj, PlayerGold.GoldBag, GoldBagExtension.DiscountMulti(Player));
        }

        void ToggleSelling()
        {
            selling = !selling;
            if (selling)
                SellItems();
            else
                ShowWares();
        }

        void ShowWares()
        {
            toggleSellText.text = "Sell";
            buyItemBag.Setup(shopItems);
            CanAfford?.Invoke(PlayerGold.GoldBag.Gold);
        }

        void SellItems() => toggleSellText.text = "Buy";

        // foreach (InventoryItem item in Player.Inventory.Items)
        //     if (item.ItemGuid.CanSell)
        //     {
        //     }
        public void Setup(string shopTitle, List<Item> items)
        {
            buyItemShowCase.Reset();
            shopTitleText.text = shopTitle;
            shopItems = items;
            selling = false;
            ShowWares();
            haveGold.GoldChanged(PlayerGold.GoldBag.Gold);
            PlayerGold.GoldBag.GoldAmountChanged += GoldChanged;
        }

        void GoldChanged(int obj) => haveGold.GoldChanged(obj);
    }
}