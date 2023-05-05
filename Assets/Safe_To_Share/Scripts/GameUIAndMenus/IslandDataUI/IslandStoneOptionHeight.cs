using Character.BodyStuff;
using Character.IslandData;
using Character.PlayerStuff;
using Safe_To_Share.Scripts.Static;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
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
        [SerializeField] AssetReference givesItem;
        [SerializeField] Button decreaseButton;

        public override void Setup(Player parPlayer, string opResult)
        {
            base.Setup(parPlayer, opResult);
            if (!IslandStonesDatas.IslandDataDict.TryGetValue(island, out var data))
                return;
            amountText.text = $"Increase by donating {DonateAmount.ConvertKg()}";
            UpdateValue(data.bodyData.GetValueOfType(bodyType));
            UpdateDecreaseButton();
        }

        void UpdateDecreaseButton() => decreaseButton.interactable = CanSteal();

        public void SetCurrentValue(float arg0)
        {
            if (!IslandStonesDatas.IslandDataDict.TryGetValue(island, out var data))
                return;
            var toInt = Mathf.RoundToInt(arg0);
            data.bodyData.SetValueOfType(bodyType, toInt);
            UpdateValue(toInt);
            slider.SetValueWithoutNotify(data.bodyData.GetValueOfType(bodyType));
            slider.maxValue = data.bodyData.GetMaxValueOfType(bodyType);
        }

        bool CanAfford(out BodyStat playerBody) =>
            player.Body.BodyStats.TryGetValue(bodyType, out playerBody) &&
            !(playerBody.BaseValue <= DonateAmount + 1);

        bool CanSteal() => player.Inventory.HasItemOfGuid(EmptyGuid);

        public override void IncreaseClick()
        {
            if (!CanAfford(out var playerBody) ||
                !IslandStonesDatas.IslandDataDict.TryGetValue(island, out var data))
                return;
            playerBody.BaseValue -= DonateAmount;
            var isMax = data.bodyData.GetValueOfType(bodyType) == data.bodyData.GetMaxValueOfType(bodyType);
            var maxValue = data.bodyData.IncreaseMaxValueOfType(bodyType);
            slider.maxValue = maxValue;
            if (isMax)
            {
                data.bodyData.SetValueOfType(bodyType, maxValue);
                slider.SetValueWithoutNotify(maxValue);
            }

            UpdateValue(data.bodyData.GetValueOfType(bodyType));
        }

        public override void DecreaseClick()
        {
            if (!CanSteal())
            {
                UpdateDecreaseButton();

                return;
            }

            if (!IslandStonesDatas.IslandDataDict.TryGetValue(island, out var data)) return;
            if (!player.Inventory.LowerItemAmountWithoutLoading(EmptyGuid)) return;
            player.Inventory.AddItem(givesItem.AssetGUID);
            var isMin = data.bodyData.GetValueOfType(bodyType) == data.bodyData.GetMinValueOfType(bodyType);
            var minValue = data.bodyData.DecreaseMinValueOfType(bodyType);
            slider.minValue = minValue;
            if (isMin)
            {
                data.bodyData.SetValueOfType(bodyType, minValue);
                slider.SetValueWithoutNotify(minValue);
                currentAmount.text = bodyType == BodyStatType.Height ? minValue.ConvertCm() : minValue.ConvertKg();
            }

            UpdateDecreaseButton();
        }

        void UpdateValue(int body)
        {
            currentAmount.text = bodyType == BodyStatType.Height ? body.ConvertCm() : body.ConvertKg();
            btnImage.color = CanAfford(out var _) ? Color.green : Color.gray;
        }
#if UNITY_EDITOR
        [SerializeField] TextMeshProUGUI title;
        protected void OnValidate() => title.text = nameof(bodyType);
#endif
    }
}