using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Character.EssenceStuff.UI
{
    public class EssenceSlider : MonoBehaviour
    {
        [SerializeField] Slider slider;
        [SerializeField] string afterValueTexT = "Masc";
        [SerializeField] TextMeshProUGUI text;

        Essence ess;
        int lastVal;

        void OnDisable()
        {
            if (ess != null)
                ess.EssenceChange -= EssChange;
        }

        public void Setup(Essence essence)
        {
            ess = essence;
            ess.EssenceChange += EssChange;
            EssChange(ess.Amount);
        }

        void EssChange(int obj)
        {
            if (obj <= 0)
                slider.value = 0f;
            else if (obj < lastVal)
                slider.value = (float)obj / lastVal;
            else
            {
                // Needs testing if look good
                slider.value = 1f;
                lastVal = obj; // First or new biggest value
            }

            text.text = $"{obj} {afterValueTexT}";
        }
    }
}