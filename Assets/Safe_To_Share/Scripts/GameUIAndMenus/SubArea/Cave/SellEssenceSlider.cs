using Character.EssenceStuff;
using Currency;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SubArea.Cave
{
    public class SellEssenceSlider: MonoBehaviour
    {
        [SerializeField] Slider slider;
        [SerializeField] TextMeshProUGUI sellFor, essenceAmount;
        [SerializeField] Button sellBtn;

        int amount = 100;

        const float SellEssenceFor = 0.361436f; // Need to test around for best value 

        Essence myEss;
        GoldBag myGold;

        public void Setup(Essence essence, GoldBag goldBag)
        {
            myEss = essence;
            myGold = goldBag;

            slider.maxValue = essence.Amount;
            amount = Mathf.Min(essence.Amount, 100);
            slider.value = amount;

            essenceAmount.text = amount.ToString();
            sellFor.text = $"{SellValue().ToString()}g"; 

            slider.onValueChanged.AddListener(ChangeAmount);
            sellBtn.onClick.AddListener(SellMyEssence);
        }
        void ChangeAmount(float arg0)
        {
            amount = Mathf.RoundToInt(arg0);
            essenceAmount.text = amount.ToString();
            sellFor.text = $"{SellValue()}g";
        }
        void SellMyEssence()
        {
            myEss.Amount -= amount;
            myGold.GainGold(SellValue());

            amount = Mathf.Min(myEss.Amount, amount);
            slider.maxValue = myEss.Amount;

            essenceAmount.text = amount.ToString();
            sellFor.text = $"{SellValue().ToString()}g";
        }

        int SellValue() => Mathf.FloorToInt(amount * SellEssenceFor);

        void OnDisable()
        {
            slider.onValueChanged.RemoveAllListeners();
            sellBtn.onClick.RemoveAllListeners();
        }
    }
}
