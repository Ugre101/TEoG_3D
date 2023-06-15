using System;
using UnityEngine;

namespace Character.EssenceStuff {
    [Serializable]
    public class Essence {
        [SerializeField] int amount;

        public int Amount {
            get => amount;
            private set {
                amount = Mathf.Max(0, value);
                EssenceValueChange?.Invoke(amount);
            }
        }

        public event Action<int> EssenceValueChange;
        public event Action EssenceTotalChange;

        public void InvokeEssenceChange() => EssenceTotalChange?.Invoke();

        public int GainEssence(int toGain) {
            Amount += toGain;
            return Amount;
        }

        public int LoseEssence(int toLose) {
            var have = Mathf.Min(toLose, Amount);
            Amount -= toLose;
            return have;
        }

        public void Clear() {
            Amount = 0;
        }
    }
}