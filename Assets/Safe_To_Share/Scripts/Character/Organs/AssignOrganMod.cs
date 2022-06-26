using System;
using System.Collections.Generic;
using Character.StatsStuff.Mods;
using UnityEngine;

namespace Character.Organs
{
    [Serializable]
    public class AssignModsToOrganContainer
    {
        [SerializeField] SexualOrganType type;
        [SerializeField] List<IntMod> organMods = new();
        [SerializeField] List<IntMod> fluidMods = new();
        [SerializeField] List<IntMod> fluidRecMods = new();

        public void Assign(BaseCharacter to)
        {
            if (!to.SexualOrgans.Containers.TryGetValue(type, out var container)) return;
            if (organMods.Count > 0)
                foreach (BaseOrgan baseOrgan in container.List)
                foreach (IntMod intMod in organMods)
                    baseOrgan.Mods.AddStatMod(intMod);
            if (fluidMods.Count > 0)
                foreach (IntMod intMod in fluidMods)
                    container.Fluid.Mods.AddStatMod(intMod);
            if (fluidRecMods.Count > 0)
                foreach (IntMod recMod in fluidRecMods)
                    container.Fluid.Recovery.Mods.AddStatMod(recMod);
        }
    }
}