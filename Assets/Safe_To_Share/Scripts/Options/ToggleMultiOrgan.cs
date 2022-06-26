using Static;
using UnityEngine;
using UnityEngine.UI;

namespace Options
{
    public class ToggleMultiOrgan : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            if (TryGetComponent(out Toggle toggle))
            {
                toggle.isOn = OptionalContent.MultiOrgan.Enabled;
                toggle.onValueChanged.AddListener(MultiOrganToggle);
            }
            else
                Debug.LogError("No toggle component");
        }

        static void MultiOrganToggle(bool arg0) => OptionalContent.MultiOrgan.Enabled = arg0;
    }
}