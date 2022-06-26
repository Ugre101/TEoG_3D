using Safe_To_Share.Scripts.CameraStuff;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.Movement.Settings.UI
{
    public class ToggleInvertThirdPersonVerticalAxis : MonoBehaviour
    {
        [SerializeField] Toggle toggle;

        void Start()
        {
            toggle.SetIsOnWithoutNotify(ThirdPersonCameraSettings.InvertVerticalAxis);
            toggle.onValueChanged.AddListener(ChangeInvert);
        }

        void OnDestroy() => toggle.onValueChanged.RemoveAllListeners();

        static void ChangeInvert(bool arg0) => ThirdPersonCameraSettings.InvertVerticalAxis = arg0;
        
    }
}