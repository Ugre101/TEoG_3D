using Movement.ECM2.Source;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.Movement.Settings.UI
{
    public class ToggleThirdPersonStrafe : MonoBehaviour
    {
        [SerializeField] Toggle toggle;

        void Start()
        {
            toggle.SetIsOnWithoutNotify(ThirdPersonMovementSettings.Strafe);
            toggle.onValueChanged.AddListener(ChangeStrafe);
        }

        void OnDestroy() => toggle.onValueChanged.RemoveAllListeners();

        static void ChangeStrafe(bool arg0) => ThirdPersonMovementSettings.Strafe = arg0;
    }
}