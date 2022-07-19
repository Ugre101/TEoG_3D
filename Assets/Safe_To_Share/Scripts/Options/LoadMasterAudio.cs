using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Options
{
    public class LoadMasterAudio : MonoBehaviour
    {
        [SerializeField] AssetReference audioMixer;
        [SerializeField] AudioControlSlider master, weather, music,footSteps;

        public void Setup() => audioMixer.LoadAssetAsync<AudioMixer>().Completed += Loaded;

        void Loaded(AsyncOperationHandle<AudioMixer> obj)
        {
            master.SetupMixer(obj.Result);
            weather.SetupMixer(obj.Result);
            music.SetupMixer(obj.Result);
            footSteps.SetupMixer(obj.Result);
        }
    }
}