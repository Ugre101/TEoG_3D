using System;
using UnityEngine;

namespace Character.PlayerStuff.Currency {
    [Serializable]
    public struct GoldSave {
        [SerializeField] int gold;

        public GoldSave(int gold) => this.gold = gold;
        public int Gold => gold;
    }
}