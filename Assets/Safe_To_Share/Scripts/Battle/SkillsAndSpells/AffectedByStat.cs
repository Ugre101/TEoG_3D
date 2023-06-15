using System;
using Character.StatsStuff;
using UnityEngine;

namespace Battle.SkillsAndSpells {
    [Serializable]
    public struct AffectedByStat {
        [SerializeField] CharStatType statType;

        [SerializeField] int divValue;

        public CharStatType StatType => statType;

        public int DivValue => divValue == 0 ? 1 : divValue;
    }
}