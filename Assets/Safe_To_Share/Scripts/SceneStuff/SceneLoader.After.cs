using System.Collections;
using AvatarStuff.Holders;
using Character;
using Character.PlayerStuff;
using Safe_To_Share.Scripts.Holders;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace SceneStuff
{
    public partial class SceneLoader
    {
        public void PreLoadAfterBattle() =>
            scenePreload = afterBattleScene.SceneReference.LoadSceneAsync(LoadSceneMode.Additive);

        public void FinishPreloadAfterBattle(Player player, BaseCharacter[] partners, BaseCharacter[] allies = null) =>
            StartCoroutine(FinishPreloadAfter(true, player, partners, allies));

        public void LoadAfterBattleFromLocation(PlayerHolder player, params BaseCharacter[] partners)
        {
            lastPos = player.transform.position;
            StartCoroutine(LoadAfterBattleDirectly(player.Player, partners));
        }

        IEnumerator LoadAfterBattleDirectly(Player player, BaseCharacter[] enemyTeam,
            BaseCharacter[] allies = null)
        {
            var op = afterBattleScene.SceneReference.LoadSceneAsync();
            StartLoadStuff();
            yield return UpdateProgressWhileSceneNotDone(op);
            if (op.Status != AsyncOperationStatus.Succeeded) yield break;
            SetNewSceneStuff(afterBattleScene, op);
            yield return afterBattleScene.AfterBattleUI.LoadSceneAsync(LoadSceneMode.Additive);
            yield return waitAFrame;
            ActionSceneLoaded?.Invoke(player, enemyTeam, allies, true);
            //AfterBattleHandler.Instance.Won(player, enemyTeam, allies);
            yield return AllDone(true);
        }

        IEnumerator FinishPreloadAfter(bool win, Player player, BaseCharacter[] enemyTeam,
            BaseCharacter[] allies = null)
        {
            StartLoadStuff();
            yield return UpdateProgressWhileSceneNotDone(scenePreload);
            if (scenePreload.Status != AsyncOperationStatus.Succeeded) yield break;
            if (battleSceneOperationHandle.IsValid())
                Addressables.UnloadSceneAsync(battleSceneOperationHandle);
            if (battleUIOperationHandle.IsValid())
                Addressables.UnloadSceneAsync(battleUIOperationHandle);
            SetNewSceneStuff(afterBattleScene, scenePreload);
            if (win)
                yield return afterBattleScene.AfterBattleUI.LoadSceneAsync(LoadSceneMode.Additive);
            else
                yield return afterBattleScene.DefeatUI.LoadSceneAsync(LoadSceneMode.Additive);

            yield return waitAFrame;
            ActionSceneLoaded?.Invoke(player, enemyTeam, allies, win);
            //AfterBattleHandler.Instance.Won(player, enemyTeam, allies);
            yield return AllDone(true);
        }


        public void LeaveDefeat(Player player)
        {
            if (InSubRealm)
            {
                if (subRealmExitOnDefeat)
                    LoadNewLocation(lastLocation, player, currentSubRealm.Exit);
                else
                    StartCoroutine(LoadSceneOp(currentSubRealm, player, lastPos));
            }
            else
                LoadLastLocation(player);
        }

        public void LeaveAfterBattle(Player player)
        {
            if (InSubRealm)
                StartCoroutine(LoadSceneOp(currentSubRealm, player, lastPos));
            else
                LoadLastLocation(player);
        }
    }
}