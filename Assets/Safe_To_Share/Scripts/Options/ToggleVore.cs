using Static;
using UnityEngine;
using UnityEngine.UI;

namespace Options
{
    public class ToggleVore : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            if (TryGetComponent(out Toggle toggle))
            {
                toggle.isOn = OptionalContent.Vore.Enabled;
                toggle.onValueChanged.AddListener(VoreToggle);
            }
            else
                Debug.LogError("No toggle component");
        }

        static void VoreToggle(bool arg0) => OptionalContent.Vore.Enabled = arg0;
    }
}
