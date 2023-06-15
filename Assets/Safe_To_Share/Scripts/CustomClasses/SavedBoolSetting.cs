using UnityEngine;

namespace CustomClasses {
    public sealed class SavedBoolSetting {
        readonly string saveName;

        bool? enabled;

        public SavedBoolSetting(string saveName, bool startValue = false) {
            this.saveName = saveName;
            if (PlayerPrefs.HasKey(saveName) is false)
                Enabled = startValue;
        }

        public bool Enabled {
            get {
                enabled ??= PlayerPrefs.GetInt(saveName, 0) == 1;
                return enabled.Value;
            }
            set {
                enabled = value;
                PlayerPrefs.SetInt(saveName, value ? 1 : 0);
            }
        }
    }
}