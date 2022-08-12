using System;
using System.Collections.Generic;
using UnityEngine;

namespace Safe_To_Share.Scripts.Farming
{
    public class PlantedPlant : MonoBehaviour
    {
        [SerializeField] GameObject plant;
        [SerializeField, Range(float.Epsilon, 10f)] float minSize = 0.1f;
        [SerializeField, Range(1, 50f)] float maxSize = 3f;
        
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
        }

        public void Load(PlantStats plantStats)
        {
            stats = plantStats;
            transform.position = plantStats.Pos;
            stats.Grown += GrowPlant;
        }
        

        void OnDestroy()
        {
            if (stats != null)
                stats.Grown -= GrowPlant;
        }

        public void GrowPlant(float growValue)
        {
            float percentDone = Mathf.Clamp(growValue,minSize,maxSize);
            plant.transform.localScale = new Vector3(percentDone, percentDone, percentDone);
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
    }
}