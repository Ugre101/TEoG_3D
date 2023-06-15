using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Character.SexStatsStuff.UI {
    public sealed class ArousalSlider : MonoBehaviour {
        [SerializeField] Slider slider;
        [SerializeField] TextMeshProUGUI arousalText;

        public void Setup(SexStats sexStats) {
            ChangeArousalSlider((float)sexStats.Arousal / sexStats.MaxArousal);
            ChangeArousalText(sexStats.Arousal);
            sexStats.ArousalChange += ChangeArousalText;
            sexStats.ArousalSliderChange += ChangeArousalSlider;
        }


        void ChangeArousalSlider(float value) => slider.value = value;

        void ChangeArousalText(int value) => arousalText.text = $"{value} Arousal";
    }
}