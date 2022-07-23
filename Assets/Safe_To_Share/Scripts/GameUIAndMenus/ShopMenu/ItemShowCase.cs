using System;
using Currency;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shop.UI
{
    public class ItemShowCase : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title, desc, value;
        [SerializeField] TextMeshProUGUI amountText, totalValue;
        [SerializeField] Slider amountSlider;
        [SerializeField] Button btn;
        bool afford;
        int amount;
        GoldBag buyerGold;
        float discountMulti;

        Item item;

        public void Reset()
        {
            title.text = string.Empty;
            desc.text = "Click on a item";
            value.text = string.Empty;
            totalValue.text = string.Empty;
            amountSlider.onValueChanged.RemoveAllListeners();
            btn.onClick.RemoveAllListeners();
        }

        void ChangeAmount(float arg0) => SetQuantity(Mathf.RoundToInt(arg0));

        void SetQuantity(int quantity)
        {
            amount = quantity;
            amountText.text = $"Quantity\n{amount}";
            int finalCost = Mathf.CeilToInt(amount * item.Value * discountMulti);
            totalValue.text = $"Final cost is {finalCost}g";
            afford = buyerGold.CanAfford(finalCost);
            totalValue.color = afford ? Color.green : Color.red;
        }

        public void Setup(Item newItem, GoldBag haveGold, float discount = 0)
        {
            Reset();
            buyerGold = haveGold;
            discountMulti = discount;
            item = newItem;
            title.text = item.Title;
            desc.text = item.Desc;
            value.text = $"{item.Value}g";
            SetQuantity(1);
            int maxAfford = Mathf.FloorToInt(haveGold.Gold / (newItem.Value * discountMulti));
            amountSlider.maxValue = maxAfford;
            amountSlider.value = 1;
            amountSlider.onValueChanged.AddListener(ChangeAmount);

            btn.onClick.AddListener(BuyItem);
        }

        public static event Action<Item, int> BuyItemInAmount;

        void BuyItem()
        {
            if (afford)
                BuyItemInAmount?.Invoke(item, amount);
            else
                ShowPopUpCantAfford();
        }

        void ShowPopUpCantAfford()
        {
        }
    }
}