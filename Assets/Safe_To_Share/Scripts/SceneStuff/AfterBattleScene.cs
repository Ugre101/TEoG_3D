using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SaveStuff {
    [CreateAssetMenu(menuName = "Scene Data/Create AfterBattleScene", fileName = "AfterBattleScene", order = 0)]
    public sealed class AfterBattleScene : GameSceneSo {
        [SerializeField] AssetReference afterBattleUI;
        [SerializeField] AssetReference defeatUI;
        public override GameSceneType SceneType => GameSceneType.AfterBattle;
        public AssetReference AfterBattleUI => afterBattleUI;

        public AssetReference DefeatUI => defeatUI;
    }
}