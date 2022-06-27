using Character.BodyStuff;
using Character.IslandData;
using Safe_To_Share.Scripts.Static;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.GameUIAndMenus.IslandDataUI
{
    public class IslandStoneOptionHeight : IslandStoneOption
    {
        const int DonateAmount = 10;
        [SerializeField] BodyStatType bodyType;
        [SerializeField] TextMeshProUGUI amountText;
        [SerializeField] Image btnImage;
        [SerializeField] Slider slider;


        void OnEnable()
        {
            if (!IslandStonesDatas.IslandDataDict.TryGetValue(island, out var data))
                return;
            amountText.text = $"Increase by donating {DonateAmount.ConvertKg()}";
            UpdateValue(data.bodyData.GetValueOfType(bodyType));
        }

        public void SetCurrentValue(float arg0)
        {
            if (!IslandStonesDatas.IslandDataDict.TryGetValue(island, out var data))
                return;
            int toInt = Mathf.RoundToInt(arg0);
            data.bodyData.SetValueOfType(bodyType, toInt);
            UpdateValue(toInt);
            slider.SetValueWithoutNotify(data.bodyData.GetValueOfType(bodyType));
            slider.maxValue = data.bodyData.GetMaxValueOfType(bodyType);
        }

        bool CanAfford(out BodyStat playerBody) =>
            player.Body.BodyStats.TryGetValue(bodyType, out playerBody) &&
            !(playerBody.BaseValue <= DonateAmount + 1);

        public override void Click()
        {
            if (!CanAfford(out var playerBody) ||
                !IslandStonesDatas.IslandDataDict.TryGetValue(island, out var data))
                return;
            playerBody.BaseValue -= DonateAmount;
            data.bodyData.IncreaseMaxValueOfType(bodyType);
            int maxValue = data.bodyData.GetMaxValueOfType(bodyType);
            slider.maxValue = maxValue;
            if (data.bodyData.GetValueOfType(bodyType) + 1 == maxValue)
            {
                data.bodyData.SetValueOfType(bodyType, maxValue);
                slider.SetValueWithoutNotify(maxValue);
            }

            UpdateValue(data.bodyData.GetValueOfType(bodyType));
        }

        void UpdateValue(int body)
        {
            currentAmount.text = body.ConvertKg();
            btnImage.color = CanAfford(out BodyStat _) ? Color.green : Color.gray;
        }
#if UNITY_EDITOR
        [SerializeField] TextMeshProUGUI title;
        protected void OnValidate() => title.text = bodyType.ToString();
#endif
    }
}