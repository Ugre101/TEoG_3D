using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.Options {
    public sealed class AudioControlSlider : MonoBehaviour {
        [SerializeField] Slider slider;

        [SerializeField] string volumeParameter = "Master Volume";
        [SerializeField] float stepMultiplier;


        AudioMixer loaded;

        void OnDestroy() => PlayerPrefs.SetFloat(volumeParameter, slider.value);
        // Start is called before the first frame update

        public void SetupMixer(AudioMixer mixer) {
            loaded = mixer;
            slider.onValueChanged.AddListener(ChangeMaster);
            slider.value = PlayerPrefs.GetFloat(volumeParameter, slider.value);
            //Load();
        }

        void ChangeMaster(float arg0) {
            var newValue = arg0 == 0 ? -80f : Mathf.Log10(arg0) * stepMultiplier;
            loaded.SetFloat(volumeParameter, newValue);
        }
    }
}