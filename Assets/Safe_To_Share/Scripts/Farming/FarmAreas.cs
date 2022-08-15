using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Safe_To_Share.Scripts.Static;
using SceneStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.Farming
{
    public static class FarmAreas
    {
        static List<FarmArea> farmAreas = new();

        public static void Initialize() => DateSystem.TickHour += TickHour;
        public static Dictionary<string, FarmArea> FarmDict = new();

        public static bool TryGetCurrentArea(out FarmArea farm)
        {
            farm = null;
            if (SceneLoader.CurrentLocationSceneGuid == null)
                return false;
            if (FarmDict.TryGetValue(SceneLoader.CurrentLocationSceneGuid, out var area))
            {
                farm = area;
                return true;
            }
            AddFarm(new FarmArea(SceneLoader.CurrentLocationSceneGuid));
            if (FarmDict.TryGetValue(SceneLoader.CurrentLocationSceneGuid, out var newArea))
            {
                farm = newArea;
                return true;
            }
            return false;
        }
        public static void AddFarm(FarmArea area)
        {
            if (farmAreas.Exists(f => f.SceneGuid == area.SceneGuid))
                return;
            farmAreas.Add(area);
            FarmDict = farmAreas.ToDictionary(a => a.SceneGuid);
        }

        static void TickHour(int ticks)
        {
            foreach (var farmArea in farmAreas)
                farmArea.TickHour(ticks);
        }
        public static IEnumerator Load(FarmSave load)
        {
            foreach (var farmArea in farmAreas)
                farmArea.Clear();

            foreach (var areaSave in load.FarmAreaSaves)
            {
                bool foundMatch = false;
                foreach (var farmArea in farmAreas)
                {
                    if (farmArea.SceneGuid == areaSave.SceneGuid)
                    {
                        foundMatch = true;
                        yield return farmArea.Load(areaSave);
                        break;
                    }
                }

                if (foundMatch is false)
                {
                    var newArea = new FarmArea(areaSave.SceneGuid);
                    yield return newArea.Load(areaSave);
                    farmAreas.Add(newArea);
                }
            }
        }

        public static FarmSave Save()
        {
            var saves = farmAreas.Select(farmArea => farmArea.Save()).ToList();
            return new FarmSave(saves);
        }

        [Serializable]
        public struct FarmSave
        {
            [field: SerializeField] public List<FarmArea.FarmAreaSave> FarmAreaSaves;

            public FarmSave(List<FarmArea.FarmAreaSave> farmAreaSaves)
            {
                FarmAreaSaves = farmAreaSaves;
            }
        }
    }
}