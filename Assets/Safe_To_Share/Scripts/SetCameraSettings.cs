using Safe_To_Share.Scripts.Options;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts
{
    public sealed class SetCameraSettings : MonoBehaviour
    {
        [SerializeField] Slider slider;

        [SerializeField] TextMeshProUGUI text;

        // Start is called before the first frame update
        void Start()
        {
            UpdateText(CameraSettings.RenderDistance);
            slider.SetValueWithoutNotify(CameraSettings.RenderDistance);
        }

        void UpdateText(float dist) => text.text = Mathf.RoundToInt(dist).ToString();

        public void ChangeRenderDist(float arg0)
        {
            CameraSettings.RenderDistance = arg0;
            UpdateText(arg0);
        }
    }
}