using UnityEngine;
using UnityEngine.UI;

namespace Options
{
    public class GlobalVolumeControll : MonoBehaviour
    {
        [SerializeField] Slider slider;
        const string GlobalAudioSave = "GlobalVolumeSave";
        void Start()
        {
            slider.value = PlayerPrefs.GetFloat(GlobalAudioSave, 1f);
            slider.onValueChanged.AddListener(ChangeGlobalVolume);
        }

        static void ChangeGlobalVolume(float arg0)
        {
            AudioListener.volume = Mathf.Clamp(arg0, 0f, 1f);
            PlayerPrefs.SetFloat(GlobalAudioSave, AudioListener.volume);
        }

        public static void LoadGlobalVolume() => AudioListener.volume = PlayerPrefs.GetFloat(GlobalAudioSave, 1f);
    }
}
