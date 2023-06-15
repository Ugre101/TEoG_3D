using CustomClasses;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.CustomClasses.UI {
    public abstract class SavedBoolToggle : MonoBehaviour {
        [SerializeField] protected Toggle toggle;
        protected abstract SavedBoolSetting SavedBool { get; }

        void Start() {
            toggle.SetIsOnWithoutNotify(SavedBool.Enabled);
            toggle.onValueChanged.AddListener(OnValueChange);
        }

        void OnDestroy() => toggle.onValueChanged.RemoveAllListeners();

        void OnValueChange(bool arg0) => SavedBool.Enabled = arg0;
    }
}