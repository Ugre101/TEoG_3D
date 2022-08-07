using System;
using System.Collections.Generic;
using System.Linq;
using SceneStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.Farming
{
    public static class FarmAreas
    {
        static List<FarmArea> farmAreas = new List<FarmArea>();

        
        public static Dictionary<string, FarmArea> FarmDict = new Dictionary<string, FarmArea>();

        public static FarmArea GetCurrentArea()
        {
            if (FarmDict.TryGetValue(SceneLoader.CurrentLocationSceneGuid, out var area)) return area;
            AddFarm(new FarmArea(SceneLoader.CurrentLocationSceneGuid));
            if (FarmDict.TryGetValue(SceneLoader.CurrentLocationSceneGuid, out var newArea)) return newArea;

            throw new NullReferenceException(); // Something went wrong

        }
        public static void AddFarm(FarmArea area)
        {
            if (farmAreas.Exists(f => f.SceneGuid == area.SceneGuid))
                return;
            farmAreas.Add(area);
            FarmDict = farmAreas.ToDictionary(a => a.SceneGuid);
        }
        public static void Load()
        {
            
        }

        public static void Save()
        {
            
        }
    }
}