using System.Collections.Generic;
using Character;
using Character.CreateCharacterStuff;
using Character.EnemyStuff;
using Character.EssenceStuff;
using Character.LevelStuff;
using Character.PlayerStuff;
using Character.VoreStuff;
using Safe_To_Share.Scripts.AfterBattle.Defeated;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle
{
    public class AfterBattleTester : MonoBehaviour
    {
#if UNITY_EDITOR

        [SerializeField] CharacterPreset playerSheet;
        [SerializeField] EnemyPreset enemySheet;

        [SerializeField] List<BasicPerk> perks = new();
        [SerializeField] List<EssencePerk> essencePerks = new();
        [SerializeField] List<VorePerk> vorePerks = new();

        [Header("Characters"), SerializeField,]
        Player player;

        [SerializeField] Enemy[] enemies;

        async void Start()
        {
            print("Start after battle test");
            if (!GameTester.GetFirstCall() || !Application.isEditor)
                return;

            await playerSheet.LoadAssets();
            player = new Player(playerSheet.NewCharacter());
            player.SexualOrgans.TickMin(120);
            foreach (EssencePerk perk in essencePerks)
                perk.GainPerk(player);
            foreach (BasicPerk basicPerk in perks)
            {
                player.LevelSystem.OwnedPerks.Add(basicPerk);
                basicPerk.PerkGainedEffect(player);
            }

            foreach (var vorePerk in vorePerks)
                vorePerk.GainPerk(player);
            BaseCharacter[] playerTeam = { player, };
            await enemySheet.LoadAssets();
            Enemy enemy = new(enemySheet.NewEnemy());
            enemies = new[] { enemy, };
            AfterBattleHandler.Instance.Won(player, enemies);
        }
#endif
    }
}