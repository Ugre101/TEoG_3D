using System.Collections;
using Battle.UI;
using Character;
using Character.PlayerStuff;
using SceneStuff;
using UnityEngine;

namespace Battle
{
    public static class BattleSceneManager
    {
        static readonly WaitForSeconds PreLoadAfterBattleDelay = new(1f);
        static bool preloading;


        public static IEnumerator PreLoadAfterBattle()
        {
            yield return PreLoadAfterBattleDelay;
            IfDontAlreadyStartPreLoad();
        }

        static void IfDontAlreadyStartPreLoad()
        {
            if (preloading) return;
            preloading = true;
            SceneLoader.Instance.PreLoadAfterBattle();
        }

        public static async void GoToAfterBattle(Player player, BaseCharacter[] enemyTeamChars)
        {
            IfDontAlreadyStartPreLoad();
            await BattleManager.Instance.Hide();
            await BattleUIManager.Instance.Hide(); // Enforece hide before load
            SceneLoader.Instance.FinishPreloadAfterBattle(player, enemyTeamChars);
            preloading = false;
        }

        public static async void GoToDefeat(Player player, BaseCharacter[] enemyTeamChars)
        {
            IfDontAlreadyStartPreLoad();
            await BattleManager.Instance.Hide();
            await BattleUIManager.Instance.Hide(); // Enforece hide before load
            SceneLoader.Instance.FinishPreloadAfterBattleDefeat(player, enemyTeamChars);
            preloading = false;
        }

        public static void Leave(Player player)
        {
            if (preloading)
                SceneLoader.Instance.UnloadPreloadAndLeave(player);
            else
                SceneLoader.Instance.ReturnToLastLocation(player);
            preloading = false;
        }

        public static void Defeat()
        {
            BattleUIManager.Instance.Defeat();
            IfDontAlreadyStartPreLoad();
        }

        public static void Victory()
        {
            BattleUIManager.Instance.Victory();
            IfDontAlreadyStartPreLoad();
        }
    }
}