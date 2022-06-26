using UnityEngine;

namespace Safe_To_Share.Scripts.CameraStuff
{
    public class FirstPersonCameraSettings : MonoBehaviour
    {
        const string SaveName = "FirstPersonCameraSensitivity";
        static float? sensitivity;
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
                PlayerPrefs.SetFloat(SaveName,value);
            }
        }
    }
}