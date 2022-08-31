using System;
using System.Collections.Generic;
using Character;
using Character.Organs;
using Items;
using UnityEngine;

namespace Safe_To_Share.Scripts.Character.Items.Item_Effects
{
    [Serializable]
    public class SexualFluidItemEffects : ItemEffect
    {
        [SerializeField] List<FluidCapOfType> fluidCapStretch = new();
        [SerializeField] List<FluidRecOfType> fluidRecMods = new();

        public override void OnUse(BaseCharacter user, string itemGuid)
        {
            foreach (FluidCapOfType fluidCapOfType in fluidCapStretch)
                if (user.SexualOrgans.Containers.TryGetValue(fluidCapOfType.organType, out var container))
                    fluidCapOfType.mods.AddMods(container.Fluid.Mods, itemGuid);

            foreach (FluidRecOfType fluidRecOfType in fluidRecMods)
                if (user.SexualOrgans.Containers.TryGetValue(fluidRecOfType.organType, out var container))
                    fluidRecOfType.mods.AddMods(container.Fluid.Recovery.Mods, itemGuid);
        }

        [Serializable]
        struct FluidCapOfType
        {
            public SexualOrganType organType;
            public AssignTempMod mods;
        }

        [Serializable]
        struct FluidRecOfType
        {
            public SexualOrganType organType;
            public AssignTempMod mods;
        }
    }
}