using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Character.EssenceStuff.UI
{
    public sealed class EssenceSlider : MonoBehaviour
    {
        [SerializeField] Slider slider;
        [SerializeField] string afterValueTexT = "Masc";
        [SerializeField] TextMeshProUGUI text;

        Essence ess;

        void OnDisable()
        {
            if (ess != null)
                ess.EssenceValueChange -= EssValueChange;
        }

        public void Setup(Essence essence)
        {
            ess = essence;
            ess.EssenceValueChange += EssValueChange;
            EssValueChange(ess.Amount);
        }

        void EssValueChange(int obj)
        {
            if (obj <= 0)
                slider.value = 0f;
            else if (obj <= slider.maxValue)
                slider.value = obj;
            else
            {
                // Needs testing if look good
                slider.maxValue = obj;
                slider.value = obj;
            }

            text.text = $"{obj} {afterValueTexT}";
        }
    }
}