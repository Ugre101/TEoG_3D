using System;
using System.Collections;
using System.Collections.Generic;
using Character;
using Character.VoreStuff;
using DormAndHome.Dorm.Buildings;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace DormAndHome.Dorm
{
    public class DormManager : MonoBehaviour
    {
        [SerializeField] List<DormMate> dormMates = new();
        [SerializeField] DormBuildings buildings;
        public DormBuildings Buildings => buildings;

        public static DormManager Instance { get; private set; }

        public int DormCapacity => 3 + 5 * Buildings.DormLodge.Level;

        public bool DormHasSpace => DormCapacity > DormMates.Count;


        public List<DormMate> DormMates => dormMates;


        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Debug.LogError("Dorm manager tried to spawn twice");
        }

        void Start()
        {
            DateSystem.TickMinute += TickDormMin;
            DateSystem.TickHour += TickDormHour;
            DateSystem.TickDay += TickDormDay;
            VoreSystem.Digested += WasMateDigested;
        }


        void WasMateDigested(int obj)
        {
            if (dormMates.Exists(m => m.Identity.ID == obj))
                dormMates.Remove(dormMates.Find(gone => gone.Identity.ID == obj));
        }

        public void AddToDorm(BaseCharacter character) => DormMates.Add(new DormMate(character));

        public void RemoveDormMate(DormMate character) => DormMates.Remove(character);

        void TickDormMin(int ticks)
        {
            foreach (DormMate mate in DormMates)
                mate.TickMin(ticks);
        }


        IEnumerable<Building> WorkPlaces()
        {
            yield return Buildings.VillageBuildings.Brothel;
        }


        void TickDormHour(int ticks)
        {
            foreach (DormMate mate in DormMates)
                mate.TickHour(ticks);

            for (int i = 0; i < ticks; i++)
            {
                buildings.DormLodge.TickBuildingEffect(DormMates);
                int hour = DateSystem.Hour + 1 - i;
                if (hour < 0)
                    hour += 24;
                switch (hour)
                {
                    case 0:
                        Buildings.Dungeon.TickBuildingEffect(DormMates);
                        break;
                    case 6:
                        Buildings.DormLodge.EssenceStone.TickBuildingEffect(DormMates);
                        Buildings.Kitchen.TickBuildingEffect(DormMates);
                        break;
                    case 7:
                        TickWork();
                        break;
                    case 8:
                        TickWork();
                        break;
                    case 9:
                        Buildings.Gym.TickBuildingEffect(DormMates);
                        break;
                    case 10:
                        TickWork();
                        break;
                    case 11:
                        TickWork();
                        break;
                    case 12:
                        Buildings.Kitchen.TickBuildingEffect(DormMates);
                        Buildings.Dungeon.TickBuildingEffect(DormMates);
                        break;
                    case 13:
                        TickWork();
                        break;
                    case 14:
                        Buildings.Gym.TickBuildingEffect(DormMates);
                        break;
                    case 15:
                        TickWork();
                        break;
                    case 16:
                        TickWork();
                        break;
                    case 17:
                        TickWork();
                        break;
                    case 18:
                        Buildings.DormLodge.EssenceStone.TickBuildingEffect(DormMates);
                        Buildings.Kitchen.TickBuildingEffect(DormMates);
                        break;
                }
            }
        }

        void TickWork()
        {
            foreach (Building workBuilding in WorkPlaces())
                workBuilding.TickBuildingEffect(DormMates);
        }

        void TickDormDay(int ticks)
        {
            foreach (DormMate mate in DormMates)
                mate.TickDay(ticks);
        }

        public DormSave Save() => new(DormMates, Buildings);

        public static event Action Loaded;

        public IEnumerator Load(DormSave toLoad)
        {
            JsonUtility.FromJsonOverwrite(toLoad.BuildingsSave, buildings);
            dormMates = new List<DormMate>();
            foreach (CharacterSave save in toLoad.Mates)
            {
                DormMate loaded = JsonUtility.FromJson<DormMate>(save.RawCharacter);
                yield return loaded.Load(save);
                loaded.Loaded();
                dormMates.Add(loaded);
            }

            Loaded?.Invoke();
        }
    }
}