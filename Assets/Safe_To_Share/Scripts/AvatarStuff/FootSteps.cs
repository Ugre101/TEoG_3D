using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AvatarStuff
{
    [RequireComponent(typeof(AudioSource))]
    public class FootSteps : MonoBehaviour
    {
        [SerializeField] AudioClip[] audioClips;
        [SerializeField] AudioClip[] giantAudioClips;
        [SerializeField] AudioSource source;

        readonly System.Random rng = new();
        bool valid;
        void Start()
        {
            if (audioClips == null || audioClips.Length == 0)
                return;
            if (source == null) 
                return;
            valid = true;
        }

        void Step()
        {
            if (!valid) return;
            source.clip = transform.lossyScale.y > 3f ? giantAudioClips[rng.Next(giantAudioClips.Length)] : audioClips[rng.Next(audioClips.Length)];
            source.Play();
        }
#if UNITY_EDITOR
        [Header("Editor stuff")]
        [SerializeField] bool validated;
        void OnValidate()
        {
            if (validated) return;
            validated = true;
            if (TryGetComponent(out AudioSource gotSource))
            {
                source = gotSource;
                source.playOnAwake = false;
            }
            var assets =  AssetDatabase.FindAssets("t:AudioClip",new[] {"Assets/Imported/Foot_Steps"});
            List<AudioClip> clips = new();
            List<AudioClip> giantClips = new();
            foreach (string asset in assets)
            {
                print(asset);
                string assetPathToGuid = AssetDatabase.GUIDToAssetPath(asset);
                print(assetPathToGuid);
                if (AssetDatabase.LoadAssetAtPath(assetPathToGuid, typeof(AudioClip)) is AudioClip clip)
                {
                    if (clip.name.ToLower().Contains("giant"))
                        giantClips.Add(clip);
                    else
                        clips.Add(clip);
                }
            }
            audioClips = clips.ToArray();
            giantAudioClips = giantClips.ToArray();

        }
#endif
    }
}