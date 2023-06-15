using System;
using UnityEngine;

namespace Character.LevelStuff {
    [Serializable]
    public abstract class LevelBaseSystem {
        [SerializeField] int exp, level = 1, points;
        protected abstract int PointsGainedPerLevel { get; }

        public int ExpNeeded => Mathf.FloorToInt(99f + Mathf.Pow(Level, 2.52f));

        public int Exp => exp;

        public int Level => level;

        public int Points {
            get => points;
            private set {
                points = Mathf.Max(0, value);
                PerkPointsChanged?.Invoke(Points);
            }
        }

        public event Action<int> ExpGained, LevelGained, PerkPointsChanged;
        public bool CanAffordWithPoints(int cost) => Points >= cost;

        public bool TryUsePoints(int cost = 1) {
            if (!CanAffordWithPoints(cost))
                return false;
            Points -= cost;
            return true;
        }

        public void GainExp(int expGain) {
            exp += expGain;
            while (exp >= ExpNeeded) {
                exp -= ExpNeeded;

                level++;
                LevelGained?.Invoke(level);

                Points += PointsGainedPerLevel;
            }

            ExpGained?.Invoke(exp);
        }
    }
}