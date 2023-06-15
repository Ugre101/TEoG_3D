using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Safe_To_Share.Scripts.Farming {
    [Serializable]
    public class PlantStats {
        public PlantStats(Save save, int maxHours) {
            Quality = save.Quality;
            Hours = save.Hours;
            Pos = save.Pos;
            PlantGuid = save.PlantGuid;
            MaxHours = maxHours;
        }

        public PlantStats(Plant plant, Vector3 pos) {
            PlantGuid = plant.Guid;
            MaxHours = plant.GrowTime;
            Pos = pos;
        }

        public float Quality { get; private set; }
        public int Hours { get; private set; }
        public Vector3 Pos { get; }
        public string PlantGuid { get; }
        public int MaxHours { get; }
        public float PercentDone => (float)Hours / MaxHours;

        public bool Done => MaxHours <= Hours;
        public event Action<float> Grown;

        public void TickHour(int hours, float qualityMod = 0f) {
            Hours += hours;
            for (var i = 0; i < hours; i++)
                Quality += Random.value * qualityMod / MaxHours;
            Grown?.Invoke(PercentDone);
        }

        public Save SaveStats() => new(Quality, Hours, Pos, PlantGuid);

        [Serializable]
        public struct Save {
            public float Quality;
            public int Hours;
            public Vector3 Pos;
            public string PlantGuid;

            public Save(float quality, int hours, Vector3 pos, string plantGuid) {
                Quality = quality;
                Hours = hours;
                Pos = pos;
                PlantGuid = plantGuid;
            }
        }
    }
}