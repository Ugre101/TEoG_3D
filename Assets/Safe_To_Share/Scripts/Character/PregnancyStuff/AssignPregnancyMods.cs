using System;
using System.Collections.Generic;
using Character.StatsStuff.Mods;
using UnityEngine;

namespace Character.PregnancyStuff {
    [Serializable]
    public class AssignPregnancyMods {
        [SerializeField] List<IntMod> fertilityMods = new();
        [SerializeField] List<IntMod> virilityMods = new();
        [SerializeField] List<IntMod> pregnancySpeedMods = new();

        public void AssignMods(BaseCharacter character) {
            if (fertilityMods.Count > 0)
                foreach (var fertilityMod in fertilityMods)
                    character.PregnancySystem.Fertility.Mods.AddStatMod(fertilityMod);
            if (virilityMods.Count > 0)
                foreach (var virilityMod in virilityMods)
                    character.PregnancySystem.Virility.Mods.AddStatMod(virilityMod);
            if (pregnancySpeedMods.Count > 0)
                foreach (var pregnancySpeedMod in pregnancySpeedMods)
                    character.PregnancySystem.PregnancySpeed.Mods.AddStatMod(pregnancySpeedMod);
        }
    }
}