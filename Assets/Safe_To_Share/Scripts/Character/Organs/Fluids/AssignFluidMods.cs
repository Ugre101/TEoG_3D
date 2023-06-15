using System;
using System.Collections.Generic;
using Character.StatsStuff.Mods;
using UnityEngine;

namespace Character.Organs.Fluids {
    [Serializable]
    public class AssignFluidMods {
        [SerializeField] List<IntMod> fluidCapStretchAll = new();
        [SerializeField] List<FluidCapOfType> fluidCapStretch = new();
        [SerializeField] List<IntMod> fluidRecModsAll = new();
        [SerializeField] List<FluidRecOfType> fluidRecMods = new();

        public void AssignMods(BaseCharacter character) {
            foreach (var fluidCapOfType in fluidCapStretch)
                fluidCapOfType.AssignMods(character);
            foreach (var intMod in fluidCapStretchAll)
            foreach (var container in character.SexualOrgans.Containers.Values)
                container.Fluid.Mods.AddStatMod(intMod);
            foreach (var fluidRecOfType in fluidRecMods)
                fluidRecOfType.AssignMods(character);
            foreach (var intMod in fluidRecModsAll)
            foreach (var container in character.SexualOrgans.Containers.Values)
                container.Fluid.Mods.AddStatMod(intMod);
        }

        [Serializable]
        struct FluidCapOfType {
            [SerializeField] SexualOrganType organType;
            [SerializeField] List<IntMod> mods;

            public void AssignMods(BaseCharacter character) {
                if (mods == null) return;
                if (!character.SexualOrgans.Containers.TryGetValue(organType, out var container)) return;
                foreach (var intMod in mods) container.Fluid.Mods.AddStatMod(intMod);
            }
        }

        [Serializable]
        struct FluidRecOfType {
            [SerializeField] SexualOrganType organType;
            [SerializeField] List<IntMod> mods;

            public void AssignMods(BaseCharacter character) {
                if (mods == null) return;
                if (!character.SexualOrgans.Containers.TryGetValue(organType, out var container)) return;
                foreach (var intMod in mods) container.Fluid.Recovery.Mods.AddStatMod(intMod);
            }
        }
    }
}