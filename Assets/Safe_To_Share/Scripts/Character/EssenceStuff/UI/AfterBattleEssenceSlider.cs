using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Character.EssenceStuff.UI {
    public sealed class AfterBattleEssenceSlider : MonoBehaviour {
        [SerializeField] TextMeshProUGUI sessionChange;

        [SerializeField] Slider essenceSlider;
        [SerializeField] TextMeshProUGUI currentEssence;
        BaseCharacter character;
        Essence essence;
        EssenceType essenceType;
        int startTot, changeTot;


        void OnDestroy() {
            if (essence != null)
                essence.EssenceTotalChange -= ValueTotalChange;
        }

        public void Setup(BaseCharacter parCharacter, EssenceType type) {
            character = parCharacter;
            essenceType = type;
            var parEssence = parCharacter.GetEssenceOfType(type);
            startTot = parCharacter.CalcTotalEssenceOfType(type);
            essence = parEssence;
            parEssence.EssenceTotalChange += ValueTotalChange;
            essenceSlider.maxValue = startTot;
            essenceSlider.value = startTot;
            sessionChange.text = $"{changeTot}";
            currentEssence.text = $"{startTot} ({essence.Amount})";
        }

        void ValueTotalChange() {
            var totalEssenceOfType = character.CalcTotalEssenceOfType(essenceType);
            changeTot = totalEssenceOfType - startTot;
            sessionChange.text = $"{changeTot}";
            sessionChange.color = changeTot == 0 ? Color.gray : changeTot < 0 ? Color.red : Color.green;

            if (essenceSlider.maxValue < totalEssenceOfType)
                essenceSlider.maxValue = totalEssenceOfType;
            essenceSlider.value = totalEssenceOfType;
            currentEssence.text = $"{totalEssenceOfType} ({essence.Amount})";
        }
    }
}