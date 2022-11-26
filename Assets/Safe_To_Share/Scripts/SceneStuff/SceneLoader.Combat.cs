using System;
using System.Collections;
using AvatarStuff.Holders;
using Character;
using Character.PlayerStuff;
using Safe_To_Share.Scripts.Holders;
using Safe_To_Share.Scripts.Static;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace SceneStuff
{
    public partial class SceneLoader
    {
        AsyncOperationHandle<SceneInstance> battleSceneOperationHandle;

        AsyncOperationHandle<SceneInstance> battleUIOperationHandle;
        bool loadingBattle, loadingBattleUI;

        bool startedCombat;

        public void LoadCombatUIIfNotAlready()
        {
            if (currentScene == battleScene)
                return;
            if (!loadingBattleUI)
                battleUIOperationHandle = battleUIScene.SceneReference.LoadSceneAsync(LoadSceneMode.Additive);
            loadingBattleUI = true;
        }

        public void LoadCombatIfNotAlready()
        {
            if (currentScene == battleScene)
                return;
            if (!loadingBattle)
                battleSceneOperationHandle = battleScene.SceneReference.LoadSceneAsync(LoadSceneMode.Additive);
            loadingBattle = true;
        }

        void PreloadCombat(GameManager.EnemyClose obj)
        {
            switch (obj)
            {
                case GameManager.EnemyClose.OutOfRange:
                    break;
                case GameManager.EnemyClose.InView:
                    LoadCombatIfNotAlready();
                    break;
                case GameManager.EnemyClose.Chasing:
                    LoadCombatUIIfNotAlready();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(obj), obj, null);
            }
        }

        void LoadCombat(Player player, params BaseCharacter[] enemy)
        {
            if (startedCombat) return;
            StartCoroutine(FinishLoadCombatScene(player, enemy));
        }

        IEnumerator FinishLoadCombatScene(Player player, BaseCharacter[] enemyTeam, BaseCharacter[] allies = null)
        {
            if (battleScene == CurrentScene)
            {
                Debug.LogError("Trying to load current scene");
                yield break;
            }

            startedCombat = true;
            if (CurrentScene is LocationSceneSo)
                lastPos = PlayerHolder.Position;
            StartLoadStuff();
            LoadCombatUIIfNotAlready();
            if (battleUIOperationHandle.Status != AsyncOperationStatus.Succeeded)
                yield return battleUIOperationHandle;
            var unloadUI = gameUI.UnLoadUI();
            var unLoadScene = CurrentLocation.SceneReference.UnLoadScene();
            while (!unLoadScene.IsDone || !unloadUI.IsDone)
            {
                LoaderScreen.UnLoadProgress(unLoadScene.PercentComplete);
                yield return null;
            }

            LoadCombatIfNotAlready();
            yield return UpdateProgressWhileSceneNotDone(battleSceneOperationHandle);
            if (battleSceneOperationHandle.Status == AsyncOperationStatus.Succeeded)
                SetNewSceneStuff(battleScene, battleSceneOperationHandle);
            yield return waitAFrame;
            ActionSceneLoaded?.Invoke(player, enemyTeam, allies, false);
            loadingBattle = false;
            loadingBattleUI = false;
            startedCombat = false;
            //  BattleManager.Instance.Setup(player.Player, enemyTeam, allies);
            yield return AllDone(true);
        }
    }
}