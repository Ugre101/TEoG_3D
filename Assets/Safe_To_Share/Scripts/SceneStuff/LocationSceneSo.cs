using System.Collections.Generic;
using System.Linq;
using SaveStuff;
using UnityEngine;

namespace SceneStuff
{
    [CreateAssetMenu(fileName = "New Scene location ref", menuName = "Scene Data/Scene location ref")]
    public class LocationSceneSo : GameSceneSo
    {
        [SerializeField] List<SubLocationSceneSo> subLocations = new();
        [SerializeField] Sprite worldMap;
        [SerializeField] string sceneName;
        public string SceneName => sceneName;
        public override GameSceneType SceneType => GameSceneType.Location;

        public Sprite WorldMap => worldMap;


        public IEnumerable<string> SaveActiveSubLocations() => from subLocationSceneSo in subLocations
            where subLocationSceneSo.SceneReference.IsValid()
            select subLocationSceneSo.Guid;
    }
}