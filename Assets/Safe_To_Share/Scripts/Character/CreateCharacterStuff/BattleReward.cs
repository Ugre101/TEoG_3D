using System;
using Safe_to_Share.Scripts.CustomClasses;
using UnityEngine;

namespace Character.CreateCharacterStuff {
    [Serializable]
    public struct BattleReward {
        [SerializeField] int goldReward;
        [SerializeField] int expReward;
        [SerializeField] RngValue rngValue;

        public int GoldReward => Mathf.RoundToInt(goldReward * rngValue.GetRandomValue);

        public int ExpReward => Mathf.RoundToInt(expReward * rngValue.GetRandomValue);
    }
}