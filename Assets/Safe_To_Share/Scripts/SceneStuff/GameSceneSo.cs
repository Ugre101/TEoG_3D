using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SaveStuff {
    /// <summary>
    ///     This class is a base class which contains what is common to all game scenes (Locations, Menus, Managers)
    /// </summary>
    [CreateAssetMenu(fileName = "New Scene ref", menuName = "Scene Data/Scene ref")]
    public class GameSceneSo : DescriptionBaseSo {
        // public AudioCueSO musicTrack;

        /// <summary>
        ///     Used by the SceneSelector tool to discern what type of scene it needs to load
        /// </summary>
        public enum GameSceneType {
            //Playable scenes
            Location, //SceneSelector tool will also load PersistentManagers and Gameplay
            Menu, //SceneSelector tool will also load Gameplay
            Battle,
            AfterBattle,
            SubLocation,
            UI,
        }

        [SerializeField] GameSceneType sceneType;
        [SerializeField] AssetReference sceneReference; //Used at runtime to load the scene from the right AssetBundle

        public virtual GameSceneType SceneType => sceneType;

        public AssetReference SceneReference => sceneReference;
    }
}