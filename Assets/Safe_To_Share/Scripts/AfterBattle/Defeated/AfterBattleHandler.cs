using Character;
using Character.EnemyStuff;
using Character.PlayerStuff;
using SceneStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle.Defeated
{
    public sealed class AfterBattleHandler : MonoBehaviour
    {
        [SerializeField] AfterBattleMain afterBattle;
        [SerializeField] DefeatedMain defeatedMain;
        [SerializeField] CustomDefeatedMain customDefeated;
        public static AfterBattleHandler Instance { get; private set; }

        void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        void Start() => SceneLoader.ActionSceneLoaded += ChooseScenario;
        void OnDestroy() => SceneLoader.ActionSceneLoaded -= ChooseScenario;

        void ChooseScenario(Player arg1, BaseCharacter[] arg2, BaseCharacter[] arg3, bool arg4)
        {
            if (arg4)
                Won(arg1, arg2, arg3);
            else
                Defeat(arg1, arg2, arg3);
        }

        public void Won(Player player, BaseCharacter[] enemies, params BaseCharacter[] allies) =>
            afterBattle.Setup(player, enemies, allies);

        public void Defeat(Player player, BaseCharacter[] enemies, params BaseCharacter[] allies)
        {
            if (enemies[0] is Enemy enemy && enemy.CustomLoseScenarios.Count > 0)
                customDefeated.Setup(player, enemies, allies);
            else
                defeatedMain.Setup(player, enemies, allies);
        }
    }
}