using System.Collections;
using System.Collections.Generic;
using Character;
using Character.CreateCharacterStuff;
using Character.EnemyStuff;
using Character.PlayerStuff;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Battle
{
    public class BattleTester : MonoBehaviour
    {
        [SerializeField] EnemyPreset enemyPreset;
        [SerializeField] CharacterPreset playerPreset;

        [SerializeField, Range(1, 3),] int enemies = 1;

        IEnumerator Start()
        {
            if (!GameTester.GetFirstCall())
                yield break;            
            
            yield return playerPreset.LoadAssets();
            BaseCharacter[] playerTeam = { new Player(playerPreset.NewCharacter()), };
            yield return enemyPreset.LoadAssets();
            List<BaseCharacter> enemyTeam = new();
            for (int i = 0; i < enemies; i++)
                enemyTeam.Add(new Enemy(enemyPreset.NewEnemy()));
            BattleManager.Instance.Setup(new Player(playerPreset.NewCharacter()), enemyTeam.ToArray(),null,false);
        }
    }
}