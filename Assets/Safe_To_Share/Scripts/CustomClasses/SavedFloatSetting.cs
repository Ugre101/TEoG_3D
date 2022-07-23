using UnityEngine;

namespace CustomClasses
{
    public class SavedFloatSetting
    {
        readonly float defaultValue;
        readonly string saveName;

        float? value;

        public SavedFloatSetting(string saveName, float defaultValue)
        {
            this.saveName = saveName;
            this.defaultValue = defaultValue;
        }

        public float Value
        {
            get
            {
                value ??= PlayerPrefs.GetFloat(saveName, defaultValue);
                return value.Value;
            }
            set
            {
                this.value = value;
                PlayerPrefs.SetFloat(saveName, value);
            }
        }
    }
}