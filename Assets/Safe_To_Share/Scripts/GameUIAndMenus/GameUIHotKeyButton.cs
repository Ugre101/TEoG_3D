using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameUIAndMenus
{
    public class GameUIHotKeyButton : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI keyText;

        [SerializeField] InputActionReference hotKey;

        // Start is called before the first frame update
        void Start() =>
            keyText.text = hotKey.action.bindings.Count > 0
                ? InputControlPath.ToHumanReadableString(hotKey.action.bindings[0].path,
                    InputControlPath.HumanReadableStringOptions.OmitDevice)
                : string.Empty;
    }
}