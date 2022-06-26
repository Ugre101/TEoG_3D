using Character.DefeatScenarios.Custom;
using Character.EssenceStuff;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.AfterBattle.Defeated.CustomScenario.UI
{
    public class DrainNodeOptions : MonoBehaviour
    {
        [SerializeField] TMP_Dropdown drain, give;
        [SerializeField] Slider drainAmount, giveAmount;
        [SerializeField] TextMeshProUGUI drainAmountText, giveAmountText;
        CustomDrainNode drainNode;

        void ChangeDrainAmount(float arg0)
        {
            drainNode.DrainBonus = Mathf.RoundToInt(arg0);
            drainAmountText.text = drainNode.DrainBonus.ToString();
        }

        void ChangeGiveAmount(float arg0)
        {
            drainNode.GiveBonus = Mathf.RoundToInt(arg0);
            giveAmountText.text = drainNode.GiveBonus.ToString();
        }


        void ChangeGiveType(int arg0) => drainNode.giveEssenceType = UgreTools.IntToEnum(arg0, DrainEssenceType.None);
        void ChangeDrainType(int arg0) => drainNode.drainEssenceType = UgreTools.IntToEnum(arg0, DrainEssenceType.None);

        public void Setup(CustomDrainNode drainNode)
        {
            this.drainNode = drainNode;
            drain.SetupTmpDropDown(drainNode.drainEssenceType, ChangeDrainType);
            drainAmountText.text = drainNode.DrainBonus.ToString();
            drainAmount.onValueChanged.RemoveAllListeners();
            drainAmount.value = drainNode.DrainBonus;
            drainAmount.onValueChanged.AddListener(ChangeDrainAmount);

            give.SetupTmpDropDown(drainNode.giveEssenceType, ChangeGiveType);
            giveAmountText.text = drainNode.GiveBonus.ToString();
            giveAmount.onValueChanged.RemoveAllListeners();
            giveAmount.value = drainNode.GiveBonus;
            giveAmount.onValueChanged.AddListener(ChangeGiveAmount);
        }
    }
}