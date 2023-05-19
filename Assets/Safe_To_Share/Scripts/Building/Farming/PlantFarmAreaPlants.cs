using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Safe_To_Share.Scripts.Farming
{
    public sealed class PlantFarmAreaPlants : MonoBehaviour
    {
        FarmArea currentArea;
        readonly List<PlantedPlant> plantedPlants = new();
        Coroutine routine;

        void Start()
        {
            if (!FarmAreas.TryGetCurrentArea(out var area)) return;
            currentArea = area;
            currentArea.Loaded += StartRefresh;
            StartCoroutine(RefreshArea());
        }

        void OnDestroy()
        {
            if (currentArea != null)
                currentArea.Loaded -= StartRefresh;
        }

        void StartRefresh()
        {
            routine = StartCoroutine(RefreshArea());
        }


        IEnumerator RefreshArea()
        {
            if (!FarmAreas.TryGetCurrentArea(out var area)) yield break;
            Dictionary<string, List<PlantStats>> loadDict = new();
            List<AsyncOperationHandle<Plant>> ops = new();
            foreach (var valuePlant in area.Plants)
                if (loadDict.TryAdd(valuePlant.PlantGuid, new List<PlantStats> { valuePlant }))
                {
                    var op = Addressables.LoadAssetAsync<Plant>(valuePlant.PlantGuid);
                    ops.Add(op);
                }
                else
                {
                    loadDict[valuePlant.PlantGuid].Add(valuePlant);
                }

            foreach (var handle in ops)
            {
                yield return handle;
                if (!loadDict.TryGetValue(handle.Result.Guid, out var values)) continue;
                var tempList = values;
                foreach
                    (var plantedPlant in plantedPlants)
                    if (plantedPlant.HasMatch(values, out var match))
                        tempList.Remove(match);
                foreach (var plantStats in tempList)
                {
                    var temp = Instantiate(handle.Result.Prefab, plantStats.Pos, quaternion.identity);
                    temp.Load(plantStats);
                    plantedPlants.Add(temp);
                }

                Addressables.Release(handle);
            }
        }
    }
}