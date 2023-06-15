using System;
using System.Collections.Generic;
using Character;
using Character.Organs;
using Items;
using UnityEngine;

namespace Safe_To_Share.Scripts.Character.Items.Item_Effects {
    [Serializable]
    public class SexualOrganItemEffect : ItemEffect {
        [SerializeField] List<SexualOrganType> affects = new();
        [SerializeField] List<AssignTempMod> mods = new();

        public override void OnUse(BaseCharacter user, string itemGuid) {
            foreach (var sexualOrganType in affects) {
                if (!user.SexualOrgans.Containers.TryGetValue(sexualOrganType, out var container)) continue;
                foreach (var baseOrgan in
                         container.BaseList) // Only adds for existing when taking item which I think is fine.
                foreach (var assignTempMod in mods)
                    assignTempMod.AddMods(baseOrgan.Mods, itemGuid);
            }
        }
    }
}