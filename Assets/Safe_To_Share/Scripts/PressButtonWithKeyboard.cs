using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts {
    public sealed class PressButtonWithKeyboard : MonoBehaviour {
        [SerializeField] Button btn;

        [SerializeField] InputAction action = new();

        // Start is called before the first frame update
        void Start() => action.performed += Click;

        void OnEnable() => action.Enable();

        void OnDisable() => action.Disable();

        void OnDestroy() => action.Dispose();

        void Click(InputAction.CallbackContext obj) => btn.onClick?.Invoke();
    }
}