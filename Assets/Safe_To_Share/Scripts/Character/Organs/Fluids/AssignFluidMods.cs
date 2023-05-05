using System;
using System.Collections.Generic;
using Character.Organs.OrgansContainers;
using Character.StatsStuff.Mods;
using UnityEngine;

namespace Character.Organs.Fluids
{
    [Serializable]
    public class AssignFluidMods
    {
        [SerializeField] List<IntMod> fluidCapStretchAll = new();
        [SerializeField] List<FluidCapOfType> fluidCapStretch = new();
        [SerializeField] List<IntMod> fluidRecModsAll = new();
        [SerializeField] List<FluidRecOfType> fluidRecMods = new();

        public void AssignMods(BaseCharacter character)
        {
            foreach (FluidCapOfType fluidCapOfType in fluidCapStretch)
                fluidCapOfType.AssignMods(character);
            foreach (IntMod intMod in fluidCapStretchAll)
            foreach (BaseOrgansContainer container in character.SexualOrgans.Containers.Values)
                container.Fluid.Mods.AddStatMod(intMod);
            foreach (FluidRecOfType fluidRecOfType in fluidRecMods)
                fluidRecOfType.AssignMods(character);
            foreach (IntMod intMod in fluidRecModsAll)
            foreach (BaseOrgansContainer container in character.SexualOrgans.Containers.Values)
                container.Fluid.Mods.AddStatMod(intMod);
        }

        [Serializable]
        struct FluidCapOfType
        {
            [SerializeField] SexualOrganType organType;
            [SerializeField] List<IntMod> mods;

            public void AssignMods(BaseCharacter character)
            {
                if (mods == null) return;
                if (!character.SexualOrgans.Containers.TryGetValue(organType, out var container)) return;
                foreach (IntMod intMod in mods) container.Fluid.Mods.AddStatMod(intMod);
            }
        }

        [Serializable]
        struct FluidRecOfType
        {
            [SerializeField] SexualOrganType organType;
            [SerializeField] List<IntMod> mods;

            public void AssignMods(BaseCharacter character)
            {
                if (mods == null) return;
                if (!character.SexualOrgans.Containers.TryGetValue(organType, out var container)) return;
                foreach (IntMod intMod in mods) container.Fluid.Recovery.Mods.AddStatMod(intMod);
            }
        }
    }
}