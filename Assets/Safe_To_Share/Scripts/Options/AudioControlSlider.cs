using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Options
{
    public class AudioControlSlider : MonoBehaviour
    {
        [SerializeField] Slider slider;

        [SerializeField] string volumeParameter = "Master Volume";
        [SerializeField] float stepMultiplier;


        AudioMixer loaded;
        // Start is called before the first frame update

        public void SetupMixer(AudioMixer mixer)
        {
            loaded = mixer;
            slider.onValueChanged.AddListener(ChangeMaster);
            slider.value = PlayerPrefs.GetFloat(volumeParameter, slider.value);
            //Load();
        }

        void OnDestroy() => PlayerPrefs.SetFloat(volumeParameter,slider.value);

        void ChangeMaster(float arg0)
        {
            float newValue = arg0 == 0 ?  -80f :Mathf.Log10(arg0) * stepMultiplier;
            loaded.SetFloat(volumeParameter, newValue);
        }
    }
}
