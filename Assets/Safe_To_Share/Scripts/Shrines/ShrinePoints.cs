using System;
using UnityEngine;

namespace Safe_To_Share.Scripts.Shrines {
    [Serializable]
    public class ShrinePoints {
        [SerializeField] int blessingPoints;

        public int BlessingPoints {
            get => blessingPoints;
            private set {
                blessingPoints = Mathf.Max(0, blessingPoints + value);
                PointsChange?.Invoke(blessingPoints);
            }
        }

        public event Action<int> PointsChange;

        public bool CanAfford(int cost) => BlessingPoints >= cost;

        public bool TrySpendPoints(int cost) {
            if (!CanAfford(cost))
                return false;
            BlessingPoints -= cost;
            return true;
        }

        public void GainPoints(int gain) => BlessingPoints += gain;
    }
}