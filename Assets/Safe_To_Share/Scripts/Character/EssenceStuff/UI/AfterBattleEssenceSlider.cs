using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Character.EssenceStuff.UI
{
    public class AfterBattleEssenceSlider : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI sessionChange;

        [SerializeField] Slider essenceSlider;
        [SerializeField] TextMeshProUGUI currentEssence;
        int startTot, changeTot;
        Essence essence;
        EssenceType essenceType;
        BaseCharacter character;
        public void Setup(BaseCharacter parCharacter,EssenceType type)
        {
            character = parCharacter;
            essenceType = type;
            Essence parEssence = parCharacter.GetEssenceOfType(type);
            startTot = parCharacter.CalcTotalEssenceOfType(type);
            print(startTot);
            essence = parEssence;
            parEssence.EssenceTotalChange += ValueTotalChange;
            essenceSlider.maxValue = startTot;
            essenceSlider.value = startTot;
            sessionChange.text = $"{changeTot}";
            currentEssence.text =  $"{startTot} ({essence.Amount})";
        }

        

        void OnDestroy()
        {
            if (essence != null)
                essence.EssenceTotalChange -= ValueTotalChange;
        }

        void ValueTotalChange()
        {
            var totalEssenceOfType = character.CalcTotalEssenceOfType(essenceType);
            changeTot = totalEssenceOfType - startTot;
            sessionChange.text = $"{changeTot}";
            sessionChange.color = changeTot == 0 ? Color.gray : changeTot < 0 ? Color.red : Color.green;

            essenceSlider.value = totalEssenceOfType;
            currentEssence.text = $"{totalEssenceOfType} ({essence.Amount})";
        }
    }
}