using Safe_To_Share.Scripts.CameraStuff;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.Movement
{
    public class ThirdPersonSensitivitySlider : MonoBehaviour
    {
        [SerializeField] Slider slider;
        [SerializeField] TextMeshProUGUI amount;

        void Start()
        {
            UpdateText(ThirdPersonCameraSettings.Sensitivity);
            slider.value = ThirdPersonCameraSettings.Sensitivity;
            slider.onValueChanged.AddListener(ChangeSensitivity);
        }

        void UpdateText(float dist) => amount.text = $"{dist:0.##}";

        void ChangeSensitivity(float arg0)
        {
            ThirdPersonCameraSettings.Sensitivity = arg0;
            UpdateText(arg0);
        }
    }
}