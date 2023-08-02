using System.Collections.Generic;
using System.Linq;
using SceneStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.Map {
    public sealed class EditorPlayerKnowsLocations : MonoBehaviour {
        [SerializeField] List<LocationSceneSo> knowLocations = new();
        [SerializeField] List<SceneTeleportExit> exits = new();

        void Start() {
            foreach (var location in knowLocations)
                KnowLocationsManager.LearnLocation(location, exits.Select(e => e.Guid).ToArray());
        }
    }
}