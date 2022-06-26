using Character.DefeatScenarios.Custom;
using Character.StatsStuff.Mods;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.AfterBattle.Defeated.CustomScenario.UI
{
    public class TempModButton : MonoBehaviour
    {
        [SerializeField] Button percentOrFlat;
        [SerializeField] TextMeshProUGUI buttonText;
        [SerializeField] Button removeMod;
        [SerializeField] Slider amountSlider;
        [SerializeField] TextMeshProUGUI amountText;
        [SerializeField] Slider durationSlider;
        [SerializeField] TextMeshProUGUI durationText;

        CustomBodyNode bodyNode;
        MakeTempMod tempMod;

        public void Setup(CustomBodyNode bodyNode, MakeTempMod tempMod)
        {
            this.bodyNode = bodyNode;
            this.tempMod = tempMod;

            percentOrFlat.onClick.RemoveAllListeners();
            buttonText.text = tempMod.ModType.ToString();
            percentOrFlat.onClick.AddListener(ToggleFlatOrPercent);

            removeMod.onClick.RemoveAllListeners();
            removeMod.onClick.AddListener(RemoveMe);

            amountSlider.onValueChanged.RemoveAllListeners();
            amountSlider.value = tempMod.Value;
            amountText.text = tempMod.Value.ToString();
            amountSlider.onValueChanged.AddListener(ChangeValue);

            durationSlider.onValueChanged.RemoveAllListeners();
            durationSlider.value = tempMod.Duration;
            durationText.text = tempMod.Duration.ToString();
            durationSlider.onValueChanged.AddListener(ChangeDuration);
        }

        void ChangeDuration(float arg0)
        {
            tempMod.Duration = Mathf.RoundToInt(arg0);
            durationText.text = Mathf.RoundToInt(arg0).ToString();
        }

        void ChangeValue(float arg0)
        {
            tempMod.Value = Mathf.RoundToInt(arg0);
            amountText.text = Mathf.RoundToInt(arg0).ToString();
        }

        void RemoveMe()
        {
            bodyNode.tempMods.Remove(tempMod);
            Destroy(gameObject);
        }

        void ToggleFlatOrPercent()
        {
            ModType toggled = tempMod.ModType == ModType.Flat ? ModType.Percent : ModType.Flat;
            tempMod.ModType = toggled;
            buttonText.text = toggled.ToString();
        }
    }
}