using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character.Organs;
using Character.Organs.Fluids;
using Character.Organs.OrgansContainers;
using UnityEngine;

namespace Character.SexStatsStuff
{
    public static class SexExtensions
    {
        public static float OrgasmFluidAmount(BaseCharacter character, SexualOrganType organType)
        {
            var fluid = GetFluidCon(character, organType);
            float value = Mathf.Clamp(Mathf.Max(fluid.FluidCurrent / 3, fluid.FluidMax / 9),0f,fluid.FluidCurrent);
            float taken = value / fluid.FluidMax * fluid.Fluid.Value;
            value = SexualOrgansExtensions.FluidAmountScaledWithHeight(value, character.Body.Height.Value);
            fluid.Fluid.DecreaseCurrentValue(taken);
            return value;
        }
        
        public static void Orgasm(BaseCharacter character, SexualOrganType outOf, IEnumerable<ForeignFluids> into)
        {
            float amount = OrgasmFluidAmount(character, outOf);
            if (!character.SexualOrgans.Containers.TryGetValue(outOf, out var con)) return;
            var fluidsArray = @into as ForeignFluids[] ?? @into.ToArray();
            if (fluidsArray.Length <= 0) return;
            float toAdd = amount / fluidsArray.Length;
            foreach (var foreignFluids in fluidsArray) 
                foreignFluids.AddFluid(con.FluidType, toAdd);
            // TODO Leakage onto body
        }
        static OrgansContainer GetFluidCon(BaseCharacter character, SexualOrganType organType)
        {
            switch (organType)
            {
                case SexualOrganType.Dick:
                case SexualOrganType.Balls:
                    return character.SexualOrgans.Balls;
                case SexualOrganType.Boobs:
                    return character.SexualOrgans.Boobs;
                case SexualOrganType.Vagina:
                    return character.SexualOrgans.Vaginas;
                case SexualOrganType.Anal:
                default:
                    throw new ArgumentOutOfRangeException(nameof(organType), organType, null);
            }
        }
    }
}