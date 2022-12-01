using Safe_To_Share.Scripts.Static;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.Options
{
    public class ToggleScat : MonoBehaviour
    {
        [SerializeField] Toggle toggle;

        void Start()
        {
            toggle.isOn = OptionalContent.Scat.Enabled;
            toggle.onValueChanged.AddListener(OnToggle);
            print(OptionalContent.Scat.Enabled);
        }

        public void OnToggle(bool value) => OptionalContent.Scat.Enabled = value;
    }
}