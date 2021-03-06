using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvatarStuff.Holders;
using Character;
using Character.EnemyStuff;
using Character.PlayerStuff;
using CustomClasses;
using DormAndHome.Dorm;
using Safe_To_Share.Scripts.Static;
using SaveStuff;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace SceneStuff
{
    public partial class SceneLoader : MonoBehaviour
    {
        static GameSceneSo currentScene;
        static LocationSceneSo lastLocation;
        [SerializeField] DropSerializableObject defaultScene;
        [SerializeField] FreePlayUILoader gameUI;
        [SerializeField] SceneTeleportExit defaultExit;
        [SerializeField] GameSceneSo battleScene;
        [SerializeField] GameSceneSo battleUIScene;
        [SerializeField] AfterBattleScene afterBattleScene;
        [SerializeField] LoaderScreen loaderScreen;
        readonly WaitForSecondsRealtime fadeDelay = new(0.3f);
        readonly WaitForSecondsRealtime teleportDelayForSubSceneLoading = new(1f);
        readonly WaitForEndOfFrame waitAFrame = new();

        Vector3 lastPos;

        AsyncOperationHandle<SceneInstance> scenePreload;

        public static LocationSceneSo CurrentLocation
        {
            get => lastLocation;
            private set
            {
                lastLocation = value;
                CurrentLocationSceneGuid = value.Guid;
            }
        }

        public static string CurrentLocationSceneGuid { get; private set; }
        public static SceneLoader Instance { get; private set; }

        public static GameSceneSo CurrentScene
        {
            get => currentScene;
            private set
            {
                currentScene = value;
                NewScene?.Invoke();
            }
        }

        public static event Action<Player, BaseCharacter[], BaseCharacter[], bool> ActionSceneLoaded;
        public LoaderScreen LoaderScreen => loaderScreen;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(this);
                Debug.LogWarning("Spawned duplicates of SceneLoader", this);
            }
        }

        void Start()
        {
            GameManager.EnemyGrowsCloser += PreloadCombat;
            PlayerHolder.LoadCombat += LoadCombat;
            PlayerHolder.LoadDormSex += LoadDormSex;
            Player.StartCombat += LoadCombat;
        }

        void LoadDormSex(PlayerHolder arg1, DormMate arg2)
        {
            StartCoroutine(LoadAfterBattleDirectly(arg1.Player, new []{arg2}));
        }


        public static event Action NewScene;

        public void ReturnToLastLocation(Player player)
        {
            StartCoroutine(CurrentLocation != null
                ? LoadSceneOp(CurrentLocation, player, lastPos)
                : LoadGoHomeSObj(player));
        }

        IEnumerator UpdateProgressWhileSceneNotDone(AsyncOperationHandle<SceneInstance> handle)
        {
            while (!handle.IsDone)
            {
                LoaderScreen.LoadProgress(handle.PercentComplete);
                yield return null;
            }
        }

        void StartLoadStuff()
        {
            LoaderScreen.StartFade();
            GameManager.Pause();
        }

        static void SetNewSceneStuff(GameSceneSo newScene, AsyncOperationHandle<SceneInstance> asyncLoad)
        {
            SceneManager.SetActiveScene(asyncLoad.Result.Scene);
            CurrentScene = newScene;
        }

        IEnumerator AllDone(bool forceFreeCursor)
        {
            yield return fadeDelay;
            LoaderScreen.StopFade();
            GameManager.Resume(forceFreeCursor);
        }

        public void LoadNewLocation(LocationSceneSo newScene, Player player, SceneTeleportExit teleportExit)
        {
            SavedEnemies.ClearEnemies();
            StartCoroutine(LoadSceneOp(newScene, player, teleportExit));
        }

        public void TeleportToExit(PlayerHolder holder, SceneTeleportExit teleportExit) =>
            StartCoroutine(HideLoadSubScenes(holder, teleportExit));

        IEnumerator HideLoadSubScenes(PlayerHolder holder, SceneTeleportExit exit)
        {
            StartLoadStuff();
            holder.gameObject.SetActive(true);
            holder.transform.position = exit.ExitPos;
            Time.timeScale = 1f;
            yield return teleportDelayForSubSceneLoading;
            LoaderScreen.StopFade();
        }

        IEnumerator LoadSceneOp(LocationSceneSo newScene, Player player, SceneTeleportExit teleportExit)
        {
            yield return BaseLoadLocationOp(newScene);
            yield return GetPlayerHolderAndReplacePlayer(player, teleportExit.ExitPos);
            yield return AllDone(false);
        }

        IEnumerator BaseLoadLocationOp(LocationSceneSo newScene)
        {
            var loaded = Addressables.LoadAssetAsync<LocationSceneSo>(newScene.Guid);
            yield return loaded;
            if (CurrentScene != null && loaded.Result.Guid == CurrentScene.Guid)
            {
                Debug.LogError("Trying to load current scene");
                yield break;
            }
            CurrentLocation = loaded.Result;
            StartLoadStuff();
            yield return fadeDelay;
            AsyncOperationHandle<SceneInstance> asyncLoad = loaded.Result.SceneReference.LoadSceneAsync();
            yield return UpdateProgressWhileSceneNotDone(asyncLoad);
            yield return gameUI.LoadGameUI();
            if (asyncLoad.Status == AsyncOperationStatus.Succeeded)
                 SetNewSceneStuff(loaded.Result, asyncLoad);
        }
        
        static IEnumerator  GetPlayerHolderAndReplacePlayer(Player player, Vector3 exitPos)
        {
            PlayerHolder holder = PlayerHolder.Instance;
            if (holder == null)
                yield break;
            Task wait = holder.ReplacePlayer(player);
            while (!wait.IsCompleted)
            {
                yield return null;
            }
            if (wait.IsFaulted)
                throw wait.Exception;
            holder.transform.position = exitPos;
        }

        IEnumerator LoadSceneOp(LocationSceneSo newScene, Player player, Vector3 playerPosition)
        {
            yield return BaseLoadLocationOp(newScene);
            yield return GetPlayerHolderAndReplacePlayer(player, playerPosition);
            yield return AllDone(false);
        }

        public void StartGame(Player tempPlayer) => GoHome(tempPlayer);

        public IEnumerator LoadSceneAndSubScenes(LocationSceneSo sceneSo, List<string> toLoad)
        {
            yield return BaseLoadLocationOp(sceneSo);
            yield return waitAFrame; // Let scene start
            yield return LoadSubScenes(toLoad);

        }

        public static IEnumerator LoadSubScenes(List<string> toLoad)
        {
            if (!toLoad.Any())
                yield break;
            List<AsyncOperationHandle<SubLocationSceneSo>> subOps = toLoad
                .Select(Addressables.LoadAssetAsync<SubLocationSceneSo>).ToList();
            List<AsyncOperationHandle<SceneInstance>> loadSubOps = new();
            foreach (AsyncOperationHandle<SubLocationSceneSo> subOp in subOps)
            {
                yield return subOp;
                if (subOp.Result.SceneActive)
                    continue;
                subOp.Result.SceneActive = true;
                subOp.Result.SceneLoaded = true;
                loadSubOps.Add(subOp.Result.SceneReference.LoadSceneAsync(LoadSceneMode.Additive));
            }

            foreach (AsyncOperationHandle<SceneInstance> loadSubOp in loadSubOps)
                yield return loadSubOp;
        }  
        public void GoHome(Player player)
        {
            if (CurrentLocation != null && CurrentLocation.Guid == defaultScene.guid)
                GetPlayerHolderAndReplacePlayer(player, defaultExit.ExitPos);
            else
                StartCoroutine(LoadGoHomeSObj(player));
        }

        IEnumerator LoadGoHomeSObj(Player player)
        {
            var asset = Addressables.LoadAssetAsync<LocationSceneSo>(defaultScene.guid);
            yield return asset;
            if (asset.Status != AsyncOperationStatus.Succeeded) yield break;
            LoadNewLocation(asset.Result, player, defaultExit);
        }


        public void FinishPreloadAfterBattleDefeat(Player player, BaseCharacter[] enemyTeamChars)
            => StartCoroutine(FinishPreloadAfter(false,player, enemyTeamChars));


        #region ReturnToLastLocation

        public void PreLoadDepLast()
        {
            if (lastLocation != null)
                Addressables.DownloadDependenciesAsync(lastLocation.SceneReference.AssetGUID, true);
        }
        public void LoadLastLocation(Player player)
        {
            if (lastLocation == null)
            {
                Addressables.LoadAssetAsync<LocationSceneSo>(defaultScene.guid).Completed += obj =>  PreloadDefault(obj, player);
                return;
            }
            scenePreload =  lastLocation.SceneReference.LoadSceneAsync();
            StartCoroutine(FinishLoadLastLocation(player));
        }

        void PreloadDefault(AsyncOperationHandle<LocationSceneSo> obj, Player player)
        {
            scenePreload = obj.Result.SceneReference.LoadSceneAsync();
            lastLocation = obj.Result;
            lastPos = new Vector3(0, 50);
            StartCoroutine(FinishLoadLastLocation(player));
        }

        IEnumerator FinishLoadLastLocation(Player player)
        {
            StartLoadStuff();
            //yield return new WaitForSecondsRealtime(0.5f);
            yield return UpdateProgressWhileSceneNotDone(scenePreload);
            if (scenePreload.Status != AsyncOperationStatus.Succeeded)
                yield break;
            yield return scenePreload.Result.ActivateAsync();
            SetNewSceneStuff(lastLocation, scenePreload);
            scenePreload.Task.Dispose();

            yield return waitAFrame;
            if (CurrentLocation != null)
                yield return GetPlayerHolderAndReplacePlayer(player, lastPos);
            else
               yield return PlayerHolder.Instance.ReplacePlayer(player);
            yield return gameUI.LoadGameUI();
            yield return AllDone(false);
        }

        #endregion

#if UNITY_EDITOR
        public async void EditorColdStart(LocationSceneSo locationSo)
        {
            CurrentLocationSceneGuid = locationSo.Guid;
            CurrentLocation = locationSo;
            currentScene = locationSo;
            await EditorSceneLoadOp(locationSo);
        }

        async Task EditorSceneLoadOp(LocationSceneSo locationSceneSo)
        {
            await locationSceneSo.SceneReference.LoadSceneAsync().Task;
            await PlayerHolder.Instance.EditorSetup();
            await gameUI.LoadGameUIAsync();
        }
#endif
    }
}