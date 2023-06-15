using System;
using System.Collections.Generic;
using Character;
using Items;
using UnityEngine;

namespace Safe_To_Share.Scripts.Character.Items.Item_Effects {
    [Serializable]
    public class SexStatsItemEffects : ItemEffect {
        [SerializeField] List<AssignTempMod> orgMods = new();
        [SerializeField] List<AssignTempMod> arousalMods = new();

        public override void OnUse(BaseCharacter user, string itemGuid) {
            foreach (var assignTempMod in orgMods)
                assignTempMod.AddMods(user.SexStats.MaxCasterOrgasms.Mods, itemGuid);
            foreach (var assignTempMod in arousalMods)
                assignTempMod.AddMods(user.SexStats.BaseMaxArousal.Mods, itemGuid);
        }
    }
}