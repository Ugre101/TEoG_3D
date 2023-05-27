using Safe_To_Share.Scripts.Movement.HoverMovement;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.Movement.Settings.UI
{
    public sealed class ToggleThirdPersonStrafe : MonoBehaviour
    {
        [SerializeField] Toggle toggle;

        void Start()
        {
            toggle.SetIsOnWithoutNotify(MovementSettings.Strafe);
            toggle.onValueChanged.AddListener(ChangeStrafe);
        }

        void OnDestroy() => toggle.onValueChanged.RemoveAllListeners();

        static void ChangeStrafe(bool arg0) => MovementSettings.Strafe = arg0;
    }
}