using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SaveStuff
{
    [CreateAssetMenu(menuName = "Scene Data/Create AfterBattleScene", fileName = "AfterBattleScene", order = 0)]
    public class AfterBattleScene : GameSceneSo
    {
        public override GameSceneType SceneType => GameSceneType.AfterBattle;


        [SerializeField] AssetReference afterBattleUI;
        [SerializeField] AssetReference defeatUI;
        public AssetReference AfterBattleUI => afterBattleUI;

        public AssetReference DefeatUI => defeatUI;
    }
}