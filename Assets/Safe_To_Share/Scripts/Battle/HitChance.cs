using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Battle {
    [Serializable]
    public class HitChance {
        [Serializable]
        public enum HitType {
            Miss, Hit, CriticalHit,
        }

        [SerializeField] List<AttackChance> attackChances = new();

        public HitType Attack() {
            if (attackChances.Count <= 0) return HitType.Hit;
            var sum = attackChances.Sum(a => a.weight);
            var value = Random.Range(0, sum);
            foreach (var chance in attackChances) {
                if (value <= chance.weight)
                    return chance.HitType;
                value -= chance.weight;
            }

            return HitType.Hit;
        }

        [Serializable]
        struct AttackChance {
            public HitType HitType;

            [Range(0f, 1f)] public float weight;
        }
    }
}