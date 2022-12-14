using CustomClasses;
using UnityEngine;

namespace Safe_To_Share.Scripts.CameraStuff
{
    public static class FirstPersonCameraSettings 
    {
        const string SaveName = "FirstPersonCameraSensitivity";
        static float? sensitivity;
        public static SavedBoolSetting FirstPersonCameraEnabled { get; } = new("FirstPersonCameraEnabled",true);

        public static SavedBoolSetting HorizontalInverted { get; } = new("FirstPersonHorizontalInverted");
        public static float Sensitivity
        {
            get
            {
                sensitivity ??= PlayerPrefs.GetFloat(SaveName, 1f);
                return sensitivity.Value;
            }
            set
            {
                sensitivity = value;
                PlayerPrefs.SetFloat(SaveName, value);
            }
        }
    }
}