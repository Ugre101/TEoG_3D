using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Safe_To_Share.Scripts.Farming
{
    [Serializable]
    public class FarmArea : ITickHour
    {
        public event Action Loaded;
        public FarmArea(string sceneGuid)
        {
            this.SceneGuid = sceneGuid;
        }

        public string SceneGuid;
        public List<PlantStats> Plants = new();

        public bool TickHour(int ticks = 1)
        {
            foreach (PlantStats plantStats in Plants)
                plantStats.TickHour(ticks);
            return false;
        }

        public FarmAreaSave Save()
        {
            return new FarmAreaSave(SceneGuid,Plants);
        }
        public IEnumerator Load(FarmAreaSave load)
        {
            Plants = new List<PlantStats>();
            Dictionary<string, List<PlantStats.Save>> plantDict = new();
            List<AsyncOperationHandle<Plant>> ops = new();
            foreach (var save in load.PlantsSave)
            {
                if (plantDict.TryGetValue(save.PlantGuid, out var list))
                    list.Add(save);
                else
                {
                    var op = Addressables.LoadAssetAsync<Plant>(save.PlantGuid);
                    ops.Add(op);
                    plantDict.TryAdd(save.PlantGuid, new List<PlantStats.Save> { save });
                }
            }

            foreach (var handle in ops)
            {
                yield return handle;
                if (handle.Status != AsyncOperationStatus.Succeeded) continue;
                if (!plantDict.TryGetValue(handle.Result.Guid, out var list)) continue;
                foreach (var save in list) 
                    Plants.Add(new PlantStats(save,handle.Result.GrowTime));
                Addressables.Release(handle);
            }
            Loaded?.Invoke();
        }

        public void AddPlant(PlantStats stats)
        {
            Plants.Add(stats);
        }
        [Serializable]
        public struct FarmAreaSave
        {
            [field: SerializeField] public string SceneGuid { get; private set; } 
            [field: SerializeField] public List<PlantStats.Save> PlantsSave { get; private set; }

            public FarmAreaSave(string sceneGuid, IEnumerable<PlantStats> plants)
            {
                SceneGuid = sceneGuid;
                var temp = plants.Select(stats => stats.SaveStats()).ToList();
                PlantsSave = temp;
            }
        }

        public void Clear() => Plants.Clear();
    }
}