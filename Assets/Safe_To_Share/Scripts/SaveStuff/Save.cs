using System;
using System.Collections.Generic;
using AvatarStuff;
using Character;
using Character.Family;
using Character.IslandData;
using Character.PlayerStuff.Currency;
using Character.VoreStuff;
using DormAndHome.Dorm;
using DormAndHome.Dorm.Buildings;
using Map;
using QuestStuff;
using Safe_To_Share.Scripts.Building;
using Safe_To_Share.Scripts.Farming;
using Safe_To_Share.Scripts.Static;
using SceneStuff;
using UnityEngine;
using Safe_To_Share.Scripts.Character.Items;

namespace SaveStuff
{
    [Serializable]
    public struct Save
    {
        [SerializeField] int lastId;
        [SerializeField] string sceneGuid;
        [SerializeField] List<string> subSceneGuid;
        [SerializeField] string player;
        [SerializeField] string date;
        [SerializeField] string locations;
        [SerializeField] string playerQuests;
        [SerializeField] string voredCharacters;
        [SerializeField] string dorm;
        [SerializeField] string dayCare;
        [SerializeField] string avatarDetails;
        [SerializeField] string islandStones;
        [SerializeField] string playerGold;
        [SerializeField] string farmsSave;
        [SerializeField] string worldInventories;
        public Save(PlayerSave character)
        {
            lastId = IDGiver.Save();
            sceneGuid = SceneLoader.CurrentLocation.Guid;
            subSceneGuid = new List<string>(SceneLoader.CurrentLocation.SaveActiveSubLocations());
            player = JsonUtility.ToJson(character);
            date = JsonUtility.ToJson(DateSystem.Save());
            locations = JsonUtility.ToJson(KnowLocationsManager.Save());
            playerQuests = JsonUtility.ToJson(QuestStuff.PlayerQuests.Save());
            voredCharacters = JsonUtility.ToJson(VoredCharacters.Save());
            dorm = JsonUtility.ToJson(DormManager.Instance.Save());
            dayCare = JsonUtility.ToJson(DayCare.Save());
            avatarDetails = AvatarDetails.Save();
            islandStones = IslandStonesDatas.Save();
            playerGold = JsonUtility.ToJson(PlayerGold.Save());
            farmsSave = JsonUtility.ToJson(FarmAreas.Save());
            worldInventories = JsonUtility.ToJson(WorldInventories.Save());
        }

        public int LastId => lastId;

        public string SceneGuid => sceneGuid;

        public PlayerSave HolderWithInventorySave => JsonUtility.FromJson<PlayerSave>(player);

        public DateSave Date => JsonUtility.FromJson<DateSave>(date);

        public KnowLocationsSave Locations => JsonUtility.FromJson<KnowLocationsSave>(locations);
        public QuestsSave PlayerQuests => JsonUtility.FromJson<QuestsSave>(playerQuests);

        public VoredCharactersSave VoreSave
        {
            get
            {
                try
                {
                    return JsonUtility.FromJson<VoredCharactersSave>(voredCharacters);
                }
                catch (Exception e)
                {
                    LoadError?.Invoke("Vored characters Failed load");
                    Console.WriteLine(e);
                    return new VoredCharactersSave(new Prey[] { });
                }
            }
        }

        public DormSave DormSave
        {
            get
            {
                try
                {
                    return JsonUtility.FromJson<DormSave>(dorm);
                }
                catch (Exception e)
                {
                    LoadError?.Invoke("Dorm Failed to load");
                    Console.WriteLine(e);
                    return new DormSave(new List<DormMate>(), new DormBuildings());
                }
            }
        }

        public DayCareSave DayCareSave
        {
            get
            {
                try
                {
                    return JsonUtility.FromJson<DayCareSave>(dayCare);
                }
                catch (Exception e)
                {
                    LoadError?.Invoke("Day care failed to load");
                    Console.WriteLine(e);
                    return new DayCareSave(new List<Child>());
                }
            }
        }

        public List<string> SubSceneGuid => subSceneGuid;

        public string SavedAvatarDetails => avatarDetails;

        public string IslandStones => islandStones;

        public int Gold
        {
            get
            {
                Debug.Log(playerGold);
                if (!string.IsNullOrEmpty(playerGold))
                {
                    GoldSave gold = JsonUtility.FromJson<GoldSave>(playerGold);
                    return gold.Gold;
                }

                if (!player.Contains("gold\":")) return 100;
                int start = player.LastIndexOf("gold\":", StringComparison.Ordinal);
                int errTest = player.LastIndexOf("goldgf\"aas:", StringComparison.Ordinal);
                Debug.Log(errTest);
                int end = player.IndexOf("}", start, StringComparison.Ordinal);
                if (start == -1 || end == -1) return 200;
                string substring = player.Substring(start, end - start);
                Debug.Log(substring);
                return int.TryParse(substring, out int res) ? res : 200;
                // if (int.TryParse(JObject))
                // "gold\\\":99999}}\"
            }
        }

        public FarmAreas.FarmSave FarmsSave => JsonUtility.FromJson<FarmAreas.FarmSave>(farmsSave);

        public WorldInventoriesSave WorldInventoriesSave
        {
            get
            {
                try
                {
                    return JsonUtility.FromJson<WorldInventoriesSave>(worldInventories);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return new WorldInventoriesSave();
                }
            }
        }

        public static event Action<string> LoadError;
    }
}