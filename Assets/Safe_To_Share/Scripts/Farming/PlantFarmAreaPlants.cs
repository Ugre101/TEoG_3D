using System;
using System.Collections;
using System.Collections.Generic;
using SaveStuff;
using SceneStuff;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Safe_To_Share.Scripts.Farming
{
    public class PlantFarmAreaPlants : MonoBehaviour
    {
        List<PlantedPlant> plantedPlants = new List<PlantedPlant>();
        Coroutine routine;

        FarmArea currentArea;
        void Start()
        {
            if (FarmAreas.TryGetCurrentArea(out var area))
            {
                currentArea = area;
                currentArea.Loaded += StartRefresh;
                StartCoroutine(RefreshArea());
            }
        }

        void OnDestroy()
        {
            if (currentArea != null)
                currentArea.Loaded -= StartRefresh;
        }

        public void AddToArea(PlantedPlant prefab, PlantStats plantStats)
        {
            if (FarmAreas.TryGetCurrentArea(out var area))
            {
                plantedPlants.Add(prefab);
            }
        }
        void StartRefresh()
        {
            routine = StartCoroutine(RefreshArea());
        }


        IEnumerator  RefreshArea()
        {
            print("Start Refresh");
            if (!FarmAreas.TryGetCurrentArea(out var area)) yield break;
            Dictionary<string, List<PlantStats>> loadDict = new();
            List<AsyncOperationHandle<Plant>> ops = new();
            foreach (PlantStats valuePlant in area.Plants)
            {
                if (loadDict.TryAdd(valuePlant.PlantGuid, new List<PlantStats> {valuePlant}))
                {
                    AsyncOperationHandle<Plant> op = Addressables.LoadAssetAsync<Plant>(valuePlant.PlantGuid);
                    ops.Add(op);
                }
                else
                {
                    loadDict[valuePlant.PlantGuid].Add(valuePlant);
                }
            }

            foreach (AsyncOperationHandle<Plant> handle in ops)
            {
                yield return handle;
                if (loadDict.TryGetValue(handle.Result.Guid, out var values))
                {
                    var tempList = values;
                    print($"Start count {tempList.Count}");
                    foreach (var plantedPlant in plantedPlants)
                    {
                        if (plantedPlant.HasMatch(values,out var match))
                        {
                            tempList.Remove(match);
                            print("Had plant match");
                        }
                    }
                    print($"Cleaned count {tempList.Count}");
                    foreach (PlantStats plantStats in tempList)
                    {
                        var temp = Instantiate(handle.Result.Prefab, plantStats.Pos,quaternion.identity);
                        temp.Load(plantStats);
                        plantedPlants.Add(temp);
                    }
                    Addressables.Release(handle);
                }

            }
        }

       
    }
}