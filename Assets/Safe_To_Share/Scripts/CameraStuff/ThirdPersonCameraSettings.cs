using CustomClasses;
using UnityEngine;

namespace Safe_To_Share.Scripts.CameraStuff {
    public static class ThirdPersonCameraSettings {
        const string SaveName = "ThirdPersonCameraSensitivity";
        const string SaveNameInvertVertical = "ThirdPersonCameraInvertVerticalAxis";
        const string SaveNameAlwaysLook = "ThirdPersonCameraAlwaysLook";
        static float? sensitivity;
        static bool? invertVerticalAxis;
        static bool? alwaysLook;
        public static SavedBoolSetting InvertHorizontalAxis = new("ThirdPersonInvertHorizontalAxis");

        public static float Sensitivity {
            get {
                sensitivity ??= PlayerPrefs.GetFloat(SaveName, 1f);
                return sensitivity.Value;
            }
            set {
                sensitivity = value;
                PlayerPrefs.SetFloat(SaveName, value);
            }
        }

        public static bool InvertVerticalAxis {
            get {
                invertVerticalAxis ??= PlayerPrefs.GetInt(SaveNameInvertVertical, 0) == 1;
                return invertVerticalAxis.Value;
            }
            set {
                invertVerticalAxis = value;
                PlayerPrefs.SetInt(SaveNameInvertVertical, value ? 1 : 0);
            }
        }

        public static bool AlwaysLook {
            get {
                alwaysLook ??= PlayerPrefs.GetInt(SaveNameAlwaysLook, 0) == 1;
                return alwaysLook.Value;
            }
            set {
                alwaysLook = value;
                PlayerPrefs.SetInt(SaveNameAlwaysLook, value ? 1 : 0);
            }
        }
    }
}