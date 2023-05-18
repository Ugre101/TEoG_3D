using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Safe_To_Share.Scripts
{
    [RequireComponent(typeof(AudioSource))]
    public sealed class RandomMusicTrack : MonoBehaviour
    {
        static readonly Random Rnd = new();

        [SerializeField] List<AudioClip> tracks = new();

        // Start is called before the first frame update
        void Start()
        {
            if (!TryGetComponent(out AudioSource player))
                return;
            player.clip = tracks[Rnd.Next(tracks.Count)];
            player.Play();
        }
    }
}