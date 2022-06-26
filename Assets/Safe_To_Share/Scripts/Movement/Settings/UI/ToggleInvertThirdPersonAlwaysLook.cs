using Safe_To_Share.Scripts.CameraStuff;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.Movement.Settings.UI
{
    public class ToggleInvertThirdPersonAlwaysLook : MonoBehaviour
    {
        [SerializeField] Toggle toggle;

        void Start()
        {
            toggle.SetIsOnWithoutNotify(ThirdPersonCameraSettings.AlwaysLook);
            toggle.onValueChanged.AddListener(ChangeLook);
        }

        void OnDestroy() => toggle.onValueChanged.RemoveAllListeners();

        static void ChangeLook(bool arg0) => ThirdPersonCameraSettings.AlwaysLook = arg0;
        
    }
}