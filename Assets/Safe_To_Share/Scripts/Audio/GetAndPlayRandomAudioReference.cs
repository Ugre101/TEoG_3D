using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Random = System.Random;

namespace Safe_To_Share.Scripts.Audio
{
    public class GetAndPlayRandomAudioReference : MonoBehaviour
    {
        [SerializeField] AudioSource audioSource;
        [SerializeField] AssetReference[] audioClipRefs;
        readonly Random rng = new();
        void Start() => PlayRandomTrack();

        void PlayRandomTrack() => audioClipRefs[rng.Next(audioClipRefs.Length)].LoadAssetAsync<AudioClip>().Completed +=
            PlayTrack;

        void PlayTrack(AsyncOperationHandle<AudioClip> obj)
        {
            audioSource.loop = true;
            audioSource.clip = obj.Result;
            audioSource.Play();
        }
    }
}