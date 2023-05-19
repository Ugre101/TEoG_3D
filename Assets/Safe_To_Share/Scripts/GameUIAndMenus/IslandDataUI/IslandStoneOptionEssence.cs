using Character.EssenceStuff;
using Character.IslandData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.GameUIAndMenus.IslandDataUI
{
    public sealed class IslandStoneOptionEssence : IslandStoneOption
    {
        const int DonateAmount = 100;
        [SerializeField] EssenceType essenceType;
        [SerializeField] TextMeshProUGUI costText;
        [SerializeField] Image btnImage;
        [SerializeField] Slider slider;


        void OnEnable()
        {
            if (!IslandStonesDatas.IslandDataDict.TryGetValue(island, out var data))
                return;
            UpdateValue(data.essenceData.GetValueOfType(essenceType));
        }

        public void SetCurrentValue(float arg0)
        {
            if (!IslandStonesDatas.IslandDataDict.TryGetValue(island, out var data))
                return;
            int toInt = Mathf.RoundToInt(arg0);
            data.essenceData.SetValueOfType(essenceType, toInt);
            UpdateValue(toInt);
            slider.SetValueWithoutNotify(data.essenceData.GetValueOfType(essenceType));
            slider.maxValue = data.essenceData.GetMaxValueOfType(essenceType);
        }

        bool CanAfford(out Essence playerEssence) =>
            player.Essence.GetEssence.TryGetValue(essenceType, out playerEssence) &&
            !(playerEssence.Amount < DonateAmount);

        public override void IncreaseClick()
        {
            if (!CanAfford(out var playerBody) ||
                !IslandStonesDatas.IslandDataDict.TryGetValue(island, out var data)) return;
            playerBody.LoseEssence( DonateAmount);
            data.essenceData.IncreaseMaxValueOfType(essenceType);
            int maxValue = data.essenceData.GetMaxValueOfType(essenceType);
            slider.maxValue = maxValue;
            if (data.essenceData.GetValueOfType(essenceType) + IslandData.EssenceData.IncreaseAmount == maxValue)
            {
                data.essenceData.SetValueOfType(essenceType, maxValue);
                slider.SetValueWithoutNotify(maxValue);
            }

            UpdateValue(data.essenceData.GetValueOfType(essenceType));
        }

        public override void DecreaseClick()
        {
            throw new System.NotImplementedException();
        }

        void UpdateValue(int ess)
        {
            currentAmount.text = $"{ess}{nameof(essenceType)}";
            btnImage.color = CanAfford(out _) ? Color.green : Color.gray;
        }
#if UNITY_EDITOR
        [SerializeField] TextMeshProUGUI title;
        void OnValidate()
        {
            title.text = nameof(essenceType);
            costText.text = $"Increase by donating {DonateAmount} {nameof(essenceType)}";
            title.color = essenceType == EssenceType.Femi ? Color.magenta : Color.blue;
        }
#endif
    }
}