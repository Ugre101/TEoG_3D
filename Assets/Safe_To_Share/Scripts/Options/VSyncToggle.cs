using UnityEngine;
using UnityEngine.UI;

namespace Options
{
    public class VSyncToggle : MonoBehaviour
    {
        private const string VsyncIsOnSaveName = "Vsynced";
        private static bool? vsync;

        private static bool Vsync
        {
            get
            {
                if (vsync.HasValue)
                    return vsync.Value;
                vsync = PlayerPrefs.GetInt(VsyncIsOnSaveName, 0) == 1;
                return vsync.Value;
            }
            set
            {
                vsync = value;
                PlayerPrefs.SetInt(VsyncIsOnSaveName, value ? 1 : 0);
            }
        }

        [SerializeField] private Toggle toggle;

        private void Start()
        {
            toggle.isOn = Vsync;
            toggle.onValueChanged.AddListener(ChangeVsync);
        }

        public static void LoadSetting() => QualitySettings.vSyncCount = Vsync ? 1 : 0;

        private static void ChangeVsync(bool on) => QualitySettings.vSyncCount = on ? 1 : 0;
    }
}