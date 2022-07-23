using System;
using UnityEngine;

namespace SaveStuff
{
    [CreateAssetMenu(fileName = "New Sub location", menuName = "Scene Data/SubLocationSceneSo", order = 0)]
    public class SubLocationSceneSo : GameSceneSo
    {
        public Action Activated;
        [NonSerialized] bool sceneActive;
        public override GameSceneType SceneType => GameSceneType.SubLocation;

        [field: NonSerialized] public bool SceneLoaded { get; set; }

        public bool SceneActive
        {
            get => sceneActive;
            set
            {
                sceneActive = value;
                Activated?.Invoke();
            }
        }
    }
}