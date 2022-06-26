using System.Collections.Generic;
using System.Linq;
using SaveStuff;
using SceneStuff;
using UnityEngine;

namespace Map
{
    public class EditorPlayerKnowsLocations : MonoBehaviour
    {
        [SerializeField] List<LocationSceneSo> knowLocations = new();
        [SerializeField] List<SceneTeleportExit> exits = new();

        void Start()
        {
            foreach (LocationSceneSo location in knowLocations)
                KnowLocationsManager.LearnLocation(location, exits.Select(e => e.Guid).ToArray());
        }
    }
}