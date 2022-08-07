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
        Coroutine routine;

        void Start()
        {
            LoadManager.LoadedSave += StartRefresh;
        }

        void OnDestroy()
        {
            LoadManager.LoadedSave -= StartRefresh;
            StopCoroutine(routine);
        }

        void StartRefresh() => routine = StartCoroutine(RefreshArea());


        static IEnumerator  RefreshArea()
        {
            if (!FarmAreas.FarmDict.TryGetValue(SceneLoader.CurrentLocationSceneGuid, out var area)) yield break;
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
                    foreach (PlantStats plantStats in values)
                    {
                        var temp = Instantiate(handle.Result.Prefab, plantStats.Pos,quaternion.identity);
                        temp.Load(plantStats);
                    }
                }

            }
        }

       
    }
}