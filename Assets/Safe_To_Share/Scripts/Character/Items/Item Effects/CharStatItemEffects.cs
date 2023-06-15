using System;
using System.Collections.Generic;
using Character;
using Character.StatsStuff;
using Items;
using UnityEngine;

namespace Safe_To_Share.Scripts.Character.Items.Item_Effects {
    [Serializable]
    public class CharStatItemEffects : ItemEffect {
        [SerializeField] List<AssignCharStatTempMod> assignCharStatTempMods = new();

        public override void OnUse(BaseCharacter user, string itemGuid) {
            foreach (var statTempMod in assignCharStatTempMods)
                if (user.Stats.GetCharStats.TryGetValue(statTempMod.statType, out var stat))
                    statTempMod.tempMods.AddMods(stat.Mods, itemGuid);
        }

        [Serializable]
        struct AssignCharStatTempMod {
            public CharStatType statType;
            public AssignTempMod tempMods;
        }
    }
}