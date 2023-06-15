using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Character.StatsStuff.HealthStuff.UI {
    public sealed class HealthSlider : MonoBehaviour {
        [SerializeField] TextMeshProUGUI text;
        [SerializeField] Slider slider;
        Health health;

        void OnDisable() => UnBindValueChange();
        void OnDestroy() => UnBindValueChange();

#if UNITY_EDITOR
        void OnValidate() {
            if (slider != null) return;
            if (TryGetComponent(out Slider healthSlider))
                slider = healthSlider;
            else
                Debug.LogWarning("Has no slider", this);
        }
#endif

        void BindValueChange() {
            health.CurrentValueChange += ChangeValue;
            health.MaxValueChange += MaxChange;
        }

        public void UnBindValueChange() {
            if (health == null)
                return;
            health.CurrentValueChange -= ChangeValue;
            health.MaxValueChange -= MaxChange;
        }

        public void Setup(Health parHealth) {
            health = parHealth;
            UpdateText();
            slider.maxValue = health.Value;
            slider.value = health.CurrentValue;
            BindValueChange();
        }

        void UpdateText() => text.text = $"{health.CurrentValue}/{health.Value}";

        void MaxChange(int obj) {
            slider.maxValue = obj;
            UpdateText();
        }

        void ChangeValue(int obj) {
            slider.value = obj;
            UpdateText();
        }
    }
}