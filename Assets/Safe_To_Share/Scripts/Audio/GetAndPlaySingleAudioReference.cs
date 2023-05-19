using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Safe_To_Share.Scripts.Audio
{
    public sealed class GetAndPlaySingleAudioReference : MonoBehaviour
    {
        [SerializeField] AudioSource audioSource;
        [SerializeField] AssetReference audioClip;

        void Start() => audioClip.LoadAssetAsync<AudioClip>().Completed += StartPlaying;

        void StartPlaying(AsyncOperationHandle<AudioClip> obj)
        {
            audioSource.loop = true;
            audioSource.clip = obj.Result;
            audioSource.Play();
        }
    }
}