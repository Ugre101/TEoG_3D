using System;
using System.Collections;
using System.Collections.Generic;
using Character.PlayerStuff;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Random = UnityEngine.Random;

namespace Safe_To_Share.Scripts.Farming
{
    public class PlantedPlant : MonoBehaviour,IInteractable
    {
        [SerializeField] GameObject plant;
        [SerializeField, Range(float.Epsilon, 10f)] float minSize = 0.1f;
        [SerializeField, Range(1, 50f)] float maxSize = 3f;
        bool placed = false;

        PlantStats stats;
        // ADD stages
        public void Plant(PlantStats plantStats)
        {
            stats = plantStats;

            if (FarmAreas.TryGetCurrentArea(out var area))
            {
                area.AddPlant(stats);
            }

            GrowPlant(0);
            stats.Grown += GrowPlant;
            placed = true;
        }

        public void Load(PlantStats plantStats)
        {
            stats = plantStats;
            transform.position = plantStats.Pos;
            stats.Grown -= GrowPlant;
            stats.Grown += GrowPlant;
            placed = true;
        }
        

        void OnDestroy()
        {
            if (stats != null)
                stats.Grown -= GrowPlant;
        }

        public void GrowPlant(float growValue)
        {
            float size = Mathf.Clamp(growValue * maxSize,minSize,maxSize);
            plant.transform.localScale = new Vector3(size, size, size);
            UpdateHoverText?.Invoke(this);
        }

        public bool HasMatch(List<PlantStats> values,out PlantStats match)
        {
            match = null;
            foreach (var plantStats in values)
            {
                if (plantStats.PlantGuid == stats.PlantGuid && plantStats.Pos == stats.Pos)
                {
                    match = plantStats;
                    GrowPlant(plantStats.Hours);
                    return true;
                }
            }

            return false;
        }

        public string HoverText(Player player)
        {
            if (!placed) return string.Empty;
            return stats.Done ? "Harvest" : $"{stats.PercentDone * 100f:##.#}% done";
        }

        bool harvesting;
        public void DoInteraction(Player player)
        {
            if (!placed || !stats.Done || harvesting) return;
            harvesting = true;
            StartCoroutine(HarvestPlant(player));
        }

        public event Action<IInteractable> UpdateHoverText;
        public event Action RemoveIInteractableHit;

        IEnumerator HarvestPlant(Player player)
        {
            var handle = Addressables.LoadAssetAsync<Plant>(stats.PlantGuid);
            yield return handle;
            if (handle.Status != AsyncOperationStatus.Succeeded) yield break;
            foreach (var plantDropItem in handle.Result.Drops)
            {
                var roll = Random.value;
                if (roll < plantDropItem.Chance)
                {
                    player.Inventory.AddItem(plantDropItem.Item);
                }
            }
            RemoveIInteractableHit?.Invoke();
            Addressables.Release(handle);
            Destroy(gameObject);
        }

    }
}
