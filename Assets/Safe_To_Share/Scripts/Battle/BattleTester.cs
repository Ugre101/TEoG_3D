using System.Collections.Generic;
using Character;
using Character.CreateCharacterStuff;
using Character.EnemyStuff;
using Character.PlayerStuff;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Battle {
    public sealed class BattleTester : MonoBehaviour {
        [SerializeField] EnemyPreset enemyPreset;
        [SerializeField] CharacterPreset playerPreset;

        [SerializeField, Range(1, 3),] int enemies = 1;

        async void Start() {
            if (!GameTester.GetFirstCall())
                return;

            await playerPreset.LoadAssets();
            BaseCharacter[] playerTeam = { new Player(playerPreset.NewCharacter()), };
            await enemyPreset.LoadAssets();
            List<BaseCharacter> enemyTeam = new();
            for (var i = 0; i < enemies; i++)
                enemyTeam.Add(new Enemy(enemyPreset.NewEnemy()));
            BattleManager.Instance.Setup(new Player(playerPreset.NewCharacter()), enemyTeam.ToArray(), null, false);
        }
    }
}