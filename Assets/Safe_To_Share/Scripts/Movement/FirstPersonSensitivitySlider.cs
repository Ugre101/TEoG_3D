using Safe_To_Share.Scripts.CameraStuff;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.Movement
{
    public class FirstPersonSensitivitySlider : MonoBehaviour
    {
        [SerializeField] Slider slider;
        [SerializeField] TextMeshProUGUI amount;

        void Start()
        {
            UpdateText(FirstPersonCameraSettings.Sensitivity);
            slider.value = FirstPersonCameraSettings.Sensitivity;
            slider.onValueChanged.AddListener(ChangeSensitivity);
        }

        void OnDestroy() => slider.onValueChanged.RemoveAllListeners();

        void UpdateText(float dist) => amount.text = $"{dist:0.##}";

        void ChangeSensitivity(float arg0)
        {
            FirstPersonCameraSettings.Sensitivity = arg0;
            UpdateText(arg0);
        }
    }
}