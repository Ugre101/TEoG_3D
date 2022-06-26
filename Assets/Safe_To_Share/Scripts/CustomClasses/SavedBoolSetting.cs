using UnityEngine;

namespace CustomClasses
{
    public class SavedBoolSetting
    {
        readonly string saveName;

        bool? enabled;
        public SavedBoolSetting(string saveName) => this.saveName = saveName;

        public bool Enabled
        {
            get
            {
                enabled ??= PlayerPrefs.GetInt(saveName, 0) == 1;
                return enabled.Value;
            }
            set
            {
                enabled = value;
                PlayerPrefs.SetInt(saveName, value ? 1 : 0);
            }
        }
    }
}