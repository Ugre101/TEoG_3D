using System;
using Character.StatsStuff;
using UnityEngine;

namespace Character.CreateCharacterStuff {
    [Serializable]
    public class StartStats {
        [SerializeField, Range(0, 99),] int strength = 10;
        [SerializeField, Range(0, 99),] int charm = 10;
        [SerializeField, Range(0, 99),] int constitution = 10;
        [SerializeField, Range(0, 99),] int intelligence = 10;
        [SerializeField, Range(0, 99),] int agility = 10;

        public Stats GetStats() => new(strength, intelligence, constitution, charm, agility);
    }
}