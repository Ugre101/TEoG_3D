using SaveStuff;
using UnityEngine;

namespace SceneStuff {
    /// <summary>
    ///     This class is a base class which contains what is common to all game scenes (Locations, Menus, Managers)
    /// </summary>
    [CreateAssetMenu(fileName = "New Scene ref", menuName = "Scene Data/Scene UI")]
    public sealed class SceneUISo : GameSceneSo {
        // public AudioCueSO musicTrack;
        public override GameSceneType SceneType => GameSceneType.UI;
    }
}