using System;
using System.Collections;
using AvatarStuff;
using AvatarStuff.Holders;
using Character;
using Character.Family;
using Character.IslandData;
using Character.PlayerStuff.Currency;
using Character.VoreStuff;
using DormAndHome.Dorm;
using Map;
using QuestStuff;
using Safe_To_Share.Scripts.Character.Items;
using Safe_To_Share.Scripts.Farming;
using Safe_To_Share.Scripts.Holders;
using Safe_To_Share.Scripts.Static;
using SceneStuff;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace SaveStuff
{
    public class LoadManager : MonoBehaviour
    {
        static readonly WaitForEndOfFrame WaitForEndOfFrame = new();
        public static LoadManager Instance { get; private set; }

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        public void QuickLoad()
        {
            if (SaveManager.LastSave.HasValue)
                Load(SaveManager.LastSave.Value);
        }

        public static event Action StartLoadingSave;
        public static event Action LoadedSave;

        public void Load(Save toLoad) => StartCoroutine(LoadOperation(toLoad));

        static IEnumerator LoadOperation(Save toLoad)
        {
            StartLoadingSave?.Invoke();
            SceneLoader.Instance.LoaderScreen.StartFade();
            GameManager.Pause();
            IDGiver.Load(toLoad.LastId);
            DateSystem.Load(toLoad.Date);
            KnowLocationsManager.Load(toLoad.Locations);
            DayCare.Load(toLoad.DayCareSave);
            AvatarDetails.Load(toLoad.SavedAvatarDetails);
            IslandStonesDatas.Load(toLoad.IslandStones);
            PlayerGold.Load(toLoad.Gold);
            WorldInventories.Load(toLoad.WorldInventoriesSave);
            yield return VoredCharacters.Load(toLoad.VoreSave);
            yield return DormManager.Instance.Load(toLoad.DormSave);
            yield return PlayerQuests.Load(toLoad.PlayerQuests);
            yield return FarmAreas.Load(toLoad.FarmsSave);
            string locGuid = toLoad.SceneGuid;
            if (SceneLoader.CurrentScene != null && locGuid == SceneLoader.CurrentScene.Guid)
            {
                yield return SceneLoader.LoadSubScenes(toLoad.SubSceneGuid);
                yield return LoadPlayer(toLoad.HolderWithInventorySave);
                yield return LoadDone();
            }
            else
                yield return LoadSceneAssetAndLoadScene(locGuid, toLoad);
        }

        static IEnumerator LoadPlayer(PlayerSave toLoad)
        {
            PlayerHolder player = PlayerHolder.Instance;
            yield return player.Load(toLoad);
            OldSaveFixer.Instance.FixPlayer(player.Player);
        }

        static IEnumerator LoadDone()
        {
            yield return WaitForEndOfFrame;
            GameManager.Resume(false);
            LoadedSave?.Invoke(); // I don't think this will fire cross scene
            SceneLoader.Instance.LoaderScreen.StopFade();
        }

        static IEnumerator LoadSceneAssetAndLoadScene(string guid, Save toLoad)
        {
            var op = Addressables.LoadAssetAsync<LocationSceneSo>(guid);
            yield return op;
            if (op.Status != AsyncOperationStatus.Succeeded) yield break;
            yield return SceneLoader.Instance.LoadSceneAndSubScenes(op.Result, toLoad.SubSceneGuid);
            yield return LoadPlayer(toLoad.HolderWithInventorySave);
            yield return LoadDone();
        }
    }
}