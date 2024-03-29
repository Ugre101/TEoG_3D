﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Character.SkillsAndSpells {
    [Serializable]
    public class AbilityBook {
        [SerializeField] string[] boundAbilities = new string[18];

        public string[] BoundAbilities {
            get => boundAbilities;
            set => boundAbilities = value;
        }

        public HashSet<string> Abilities { get; private set; } = new();

        public void Load(List<string> guids) {
            Abilities = new HashSet<string>();
            foreach (var loadSavedGuid in guids)
                if (!string.IsNullOrEmpty(loadSavedGuid))
                    Abilities.Add(loadSavedGuid);
        }

        public bool KnowAbility(string ability) => Abilities.Contains(ability);

        public void LearnAbility(string ability) => Abilities.Add(ability);
    }
}