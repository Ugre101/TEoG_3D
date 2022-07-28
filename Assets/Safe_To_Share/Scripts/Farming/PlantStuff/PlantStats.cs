using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Safe_To_Share.Scripts.Farming
{
    [Serializable]
    public class PlantStats
    {
        public PlantStats(Vector3 pos,string plantGuid,int maxHours)
        {
            Pos = pos;
            PlantGuid = plantGuid;
            MaxHours = maxHours;
        }

        public PlantStats(Save save,int maxHours)
        {
            Quality = save.Quality;
            Hours = save.Hours;
            Pos = save.Pos;
            PlantGuid = save.PlantGuid;
            MaxHours = maxHours;
        }
        public float Quality { get; private set; }
        public int Hours { get; private set; }
        public Vector3 Pos { get; }
        public string PlantGuid { get; }
        public int MaxHours { get; }

        public void TickHour(int hours, float qualityMod = 0f)
        {
            Hours += hours;
            for (int i = 0; i < hours; i++)
            {
                Quality += Random.value * qualityMod / MaxHours;
            }
        }
        public struct  Save
        {
            public float Quality;
            public int Hours;
            public Vector3 Pos;
            public string PlantGuid;
        }
    }
}