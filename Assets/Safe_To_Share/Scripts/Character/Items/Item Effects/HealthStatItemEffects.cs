using System;
using System.Collections.Generic;
using Character;
using Character.StatsStuff.HealthStuff;
using Items;
using UnityEngine;

namespace Safe_To_Share.Scripts.Character.Items.Item_Effects {
    [Serializable]
    public class HealthStatItemEffects : ItemEffect {
        [SerializeField] List<AssignHealthStatTempMod> assignCharStatTempMods = new();

        public override void OnUse(BaseCharacter user, string itemGuid) {
            foreach (var statTempMod in assignCharStatTempMods)
                switch (statTempMod.healthTypes) {
                    case HealthTypes.Health:
                        statTempMod.tempMods.AddMods(user.Stats.Health.Mods, itemGuid);
                        break;
                    case HealthTypes.WillPower:
                        statTempMod.tempMods.AddMods(user.Stats.WillPower.Mods, itemGuid);
                        break;
                    case HealthTypes.HealthRecovery:
                        statTempMod.tempMods.AddMods(user.Stats.Health.IntRecovery.Mods, itemGuid);
                        break;
                    case HealthTypes.WillPowerRecovery:
                        statTempMod.tempMods.AddMods(user.Stats.WillPower.IntRecovery.Mods, itemGuid);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
        }

        [Serializable]
        struct AssignHealthStatTempMod {
            public HealthTypes healthTypes;
            public AssignTempMod tempMods;
        }
    }
}