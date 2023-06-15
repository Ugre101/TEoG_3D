using System;
using UnityEngine;

namespace Currency {
    [Serializable]
    public class GoldBag {
        [SerializeField] int gold;

        public GoldBag(int startGold) => gold = startGold;

        public int Gold {
            get => gold;
            private set {
                gold = value;
                GoldAmountChanged?.Invoke(gold);
            }
        }

        public event Action<int> GoldAmountChanged;
        public bool CanAfford(int cost) => Gold >= cost;

        public bool TryToBuy(int cost) {
            if (!CanAfford(cost))
                return false;
            Gold -= cost;
            return true;
        }

        public void GainGold(int goldGain) => Gold += goldGain;

        public void Load(int toLoadGold) => Gold = toLoadGold;
    }
}