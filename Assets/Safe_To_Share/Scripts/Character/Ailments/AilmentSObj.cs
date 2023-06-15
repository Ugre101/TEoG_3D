using System;
using System.Collections.Generic;
using Character.StatsStuff.Mods;
using UnityEngine;

namespace Character.Ailments {
    [CreateAssetMenu(menuName = "Create AilmentSObj", fileName = "AilmentSObj", order = 0)]
    public sealed class AilmentSObj : ScriptableObject, IComparable<AilmentSObj> {
        [field: SerializeField] public string GUID { get; private set; }

        [field: SerializeField, Range(0f, 1f),]
        public float ThreesHold { get; private set; } = 0.5f;

        [SerializeField] List<IntMod> mods = new();
        [SerializeField, Range(1, 72),] int duration = 8;

        void OnValidate() {
            if (string.IsNullOrEmpty(GUID))
                GUID = Guid.NewGuid().ToString();
        }

        public int CompareTo(AilmentSObj ailment) {
            if (ailment == null)
                return 1;
            return ailment.ThreesHold.CompareTo(ThreesHold);
        }

        public bool Gain(BaseCharacter character) => false;

        public bool Cure(BaseCharacter character) => false;
    }
}