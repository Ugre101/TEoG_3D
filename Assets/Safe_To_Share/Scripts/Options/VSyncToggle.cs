using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.Options
{
    public sealed class VSyncToggle : MonoBehaviour
    {
        const string VsyncIsOnSaveName = "Vsynced";
        static bool? vsync;

        [SerializeField] Toggle toggle;

        static bool Vsync
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

        void Start()
        {
            toggle.isOn = Vsync;
            toggle.onValueChanged.AddListener(ChangeVsync);
        }

        public static void LoadSetting() => QualitySettings.vSyncCount = Vsync ? 1 : 0;

        static void ChangeVsync(bool on) => QualitySettings.vSyncCount = on ? 1 : 0;
    }
}