using System;
using UnityEngine;

namespace Safe_To_Share.Scripts.Farming
{
    public class PlantedPlant : MonoBehaviour
    {
        [SerializeField] GameObject plant;
        [SerializeField, Range(float.Epsilon, 1f)] float minSize = 0.1f;
        [SerializeField, Range(1, 5f)] float maxSize = 3f;
        
        PlantStats stats;
        // ADD stages
        public void Plant(PlantStats plantStats)
        {
            stats = plantStats;
            FarmAreas.GetCurrentArea().AddPlant(stats);
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
    }
}