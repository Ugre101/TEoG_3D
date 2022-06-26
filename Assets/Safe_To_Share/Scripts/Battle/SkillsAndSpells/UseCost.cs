using System;
using UnityEngine;

namespace Battle.SkillsAndSpells
{
    [Serializable]
    public struct UseCost
    {
        public enum CostType
        {
            Stamina,
            Mana,
            Health,
            WillPower,
            Gold,
        }

        [SerializeField] CostType type;

        [SerializeField] int cost;

        public CostType Type => type;

        public int Cost => cost;
    }
}