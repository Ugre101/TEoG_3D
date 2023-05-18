using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Character.BodyStuff;
using Character.CreateCharacterStuff;
using Character.PlayerStuff;
using Map;
using QuestStuff;
using SceneStuff;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.StartScene
{
    public sealed class SetupPlayer : MonoBehaviour
    {
        [SerializeField] CharacterPreset characterPreset;
        [SerializeField] TMP_InputField firstName, lastName;
        [SerializeField] Button startBtn;
        [SerializeField] TextMeshProUGUI heightAndWeight;
        [SerializeField] Toggle impToggle;
        [SerializeField] AssetReference mainQuest;

        [SerializeField] List<LearnStartLocation> startMaps = new();
        [SerializeField] GameObject page1, page2;
        [SerializeField] SetupGender setupGender;
        [SerializeField] SetupBackGround background;
        QuestInfo loaded;
        Player tempPlayer;

        // Start is called before the first frame update
        async void Start()
        {
            await LoadPlayer();

            startBtn.onClick.AddListener(LaunchGame);
            background.Setup();
            //  LoadAssets();
        }

        void OnEnable()
        {
            page1.SetActive(true);
            page2.SetActive(false);
        }

        void LaunchGame() => StartCoroutine(StartGame());

        public void LoadAssets()
        {
            startBtn.gameObject.SetActive(false);
            mainQuest.LoadAssetAsync<QuestInfo>().Completed += QuestLoaded;
            background.LoadStartPerk();
            setupGender.SetupGenderDropDown();
        }

        async Task LoadPlayer()
        {
            await characterPreset.LoadAssets();
            tempPlayer = new Player(characterPreset.NewCharacter());
            firstName.onValueChanged.AddListener(tempPlayer.Identity.ChangeFirstName);
            lastName.onValueChanged.AddListener(tempPlayer.Identity.ChangeLastName);
            UpdateUnits();
            impToggle.onValueChanged.AddListener(arg0 => UpdateUnits());
        }

        void UpdateUnits() => heightAndWeight.text = tempPlayer.Body.HeightAndWeight();

        void QuestLoaded(AsyncOperationHandle<QuestInfo> obj)
        {
            loaded = obj.Result;
            startBtn.gameObject.SetActive(true);
        }

        IEnumerator StartGame()
        {
            startBtn.gameObject.SetActive(false);
            PlayerQuests.AddQuest(loaded);
            FirstStartHelper.ShowHelp = true;

            foreach (LearnStartLocation startMap in startMaps)
                KnowLocationsManager.LearnLocation(startMap.Guid, startMap.ExitGuids);
            yield return setupGender.SetStartGender(tempPlayer);
            yield return background.GainPerk(tempPlayer);
            SceneLoader.Instance.StartGame(tempPlayer);
        }

        [Serializable]
        struct LearnStartLocation
        {
            [SerializeField] string guid;
            [SerializeField] string[] exitGuids;

            public LearnStartLocation(string guid, string[] exitGuids)
            {
                this.guid = guid;
                this.exitGuids = exitGuids;
            }

            public string Guid => guid;

            public string[] ExitGuids => exitGuids;
        }

# if UNITY_EDITOR

        void OnValidate()
        {
            startMaps = new List<LearnStartLocation>();
            foreach (StartLocation startLocation in startLocations)
                startMaps.Add(new LearnStartLocation(startLocation.StartLoc.Guid, startLocation.Exits));
        }

        [SerializeField] List<StartLocation> startLocations = new();

        [Serializable]
        struct StartLocation
        {
            [SerializeField] LocationSceneSo startLoc;
            [SerializeField] SceneTeleportExit[] exits;

            public LocationSceneSo StartLoc => startLoc;

            public string[] Exits => exits.Select(exit => exit.Guid).ToArray();
        }

#endif
    }
}