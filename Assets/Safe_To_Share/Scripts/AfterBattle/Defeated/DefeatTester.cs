using System.Collections.Generic;
using Character;
using Character.CreateCharacterStuff;
using Character.EnemyStuff;
using Character.EssenceStuff;
using Character.PlayerStuff;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle.Defeated
{
    public class DefeatTester : MonoBehaviour
    {
        [SerializeField] CharacterPreset playerSheet;
        [SerializeField] EnemyPreset enemySheet;

        [SerializeField] List<EssencePerk> essencePerks = new();
        [SerializeField] List<EssencePerk> enemyEssencePerks = new();

        async void Start()
        {
            if (!GameTester.GetFirstCall()) return;
            await playerSheet.LoadAssets();
            Player player = new(playerSheet.NewCharacter());
            foreach (EssencePerk perk in essencePerks)
                perk.GainPerk(player);
            BaseCharacter[] playerTeam = { player, };
            await enemySheet.LoadAssets();
            Enemy enemy = new(enemySheet.NewEnemy());
            foreach (EssencePerk enemyEssencePerk in enemyEssencePerks)
                enemyEssencePerk.GainPerk(enemy);
            BaseCharacter[] enemyTeam = { enemy, };
            AfterBattleHandler.Instance.Defeat(player, enemyTeam);
            // DefeatedMain.Instance.Setup(player, enemyTeam);
        }
    }
}