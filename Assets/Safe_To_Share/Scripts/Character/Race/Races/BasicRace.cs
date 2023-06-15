using System;
using System.Collections.Generic;
using Character.BodyStuff;
using Character.StatsStuff.Mods;
using Safe_to_Share.Scripts.CustomClasses;
using UnityEngine;

namespace Character.Race.Races {
    [CreateAssetMenu(fileName = "Basic race", menuName = "Character/Races/New basic race")]
    public sealed class BasicRace : SObjSavableTitleDescIcon {
        [SerializeField] RaceMods primaryRaceMods;
        [SerializeField] RaceMods secondaryRaceMods;
        public float AverageHeight { get; set; } = 160;

        public void AddPrimaryRaceMods(BaseCharacter character) {
            foreach (var statMod in primaryRaceMods.StatMods)
                if (character.Stats.GetCharStats.TryGetValue(statMod.Stat, out var charStat))
                    charStat.Mods.AddStatMod(statMod.Mod);
            foreach (var bodyMod in primaryRaceMods.BodyMods)
                if (character.Body.BodyStats.TryGetValue(bodyMod.Type, out var bodyStat))
                    bodyStat.Mods.AddStatMod(bodyMod.Mod);
        }

        public void AddSecondaryRaceMods(BaseCharacter character) {
            foreach (var statMod in secondaryRaceMods.StatMods)
                if (character.Stats.GetCharStats.TryGetValue(statMod.Stat, out var charStat))
                    charStat.Mods.AddStatMod(statMod.Mod);
            foreach (var bodyMod in secondaryRaceMods.BodyMods)
                if (character.Body.BodyStats.TryGetValue(bodyMod.Type, out var bodyStat))
                    bodyStat.Mods.AddStatMod(bodyMod.Mod);
        }

        public void RemovePrimaryRaceMods(BaseCharacter character) {
            foreach (var statMod in primaryRaceMods.StatMods)
                if (character.Stats.GetCharStats.TryGetValue(statMod.Stat, out var charStat))
                    charStat.Mods.RemoveStatMod(statMod.Mod);
            foreach (var bodyMod in primaryRaceMods.BodyMods)
                if (character.Body.BodyStats.TryGetValue(bodyMod.Type, out var bodyStat))
                    bodyStat.Mods.RemoveStatMod(bodyMod.Mod);
        }

        public void RemoveSecondaryRaceMod(BaseCharacter character) {
            foreach (var statMod in secondaryRaceMods.StatMods)
                if (character.Stats.GetCharStats.TryGetValue(statMod.Stat, out var charStat))
                    charStat.Mods.RemoveStatMod(statMod.Mod);
            foreach (var bodyMod in secondaryRaceMods.BodyMods)
                if (character.Body.BodyStats.TryGetValue(bodyMod.Type, out var bodyStat))
                    bodyStat.Mods.RemoveStatMod(bodyMod.Mod);
        }
    }

    [Serializable]
    public struct RaceMods {
        public List<AssignCharStatMod> StatMods;
        public List<AssignBodyMod> BodyMods;
    }
}