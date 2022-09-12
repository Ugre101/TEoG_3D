using System.Collections.Generic;
using System.Linq;
using Character.Organs.Fluids;
using Character.Organs.OrgansContainers;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Character.Organs
{
    public static class SexualOrgansExtensions
    {
        public static int FluidMaxValueCylinder(this IEnumerable<BaseOrgan> organs, SexualFluid sexualFluid)
        {
            float sexualFluidValue = sexualFluid.Value / 100f;
            return Mathf.CeilToInt(CylinderCalc(organs) * sexualFluidValue);
        }

        public static int FluidCurrentValueCylinder(this IEnumerable<BaseOrgan> organs, SexualFluid sexualFluid)
        {
            float sexualFluidCurrentValue = sexualFluid.CurrentValue / 100f;
            return Mathf.CeilToInt(CylinderCalc(organs) * sexualFluidCurrentValue);
        }

        public static int FluidMaxValueSphere(this IEnumerable<BaseOrgan> organs, SexualFluid sexualFluid)
        {
            float sexualFluidValue = sexualFluid.Value / 100000f;
            return Mathf.CeilToInt(SphereCalc(organs) * sexualFluidValue);
        }

        public static float FluidAmountScaledWithHeight(float fluidAmount, float height)
            => fluidAmount * HeightScaleValue(height);

        public static string FluidAmountInText(float amount, float height) =>
            FluidAmountScaledWithHeight(amount, height).ConvertCl();

        static float CylinderCalc(IEnumerable<BaseOrgan> organs)
        {
            IEnumerable<BaseOrgan> baseOrgans = organs as BaseOrgan[] ?? organs.ToArray();
            return Mathf.PI * Mathf.Pow(baseOrgans.Sum(o => o.Value) * 0.8f, 2) * baseOrgans.Sum(o => o.Value);
        }

        // 4/3 * PI * r3
        static float SphereCalc(IEnumerable<BaseOrgan> organs) =>
            4f / 3f * Mathf.PI * Mathf.Pow(organs.Sum(o => o.Value), 3f);

        public static float ScaledWithHeight(this BaseOrgan organ, float height)
            => organ.Value * HeightScaleValue(height);

        static float HeightScaleValue(float height) => 0.1f + height / 177f;

        public static int TotalEssenceCost(this OrgansContainer organs)
        {
            int cost = 0;
            for (int i = 0; i < organs.List.Count(); i++) 
                cost += organs.GrowNewCostAt(i);
            foreach (var baseOrgan in organs.List)
                for (var i = 1; i < baseOrgan.BaseValue; i++)
                    cost += baseOrgan.GrowCostAt(i);
            return cost;
        }

        public static string OrganDesc(SexualOrganType type, BaseOrgan organ, bool capitalLeter = true) => type switch
        {
            SexualOrganType.Dick => $"{(capitalLeter ? "A" : "a")} {organ.Value.ConvertCm()} long dick",
            SexualOrganType.Balls => $"{(capitalLeter ? "A" : "a")} pair of {organ.Value.ConvertCm()} wide balls",
            SexualOrganType.Boobs => $"{(capitalLeter ? "A" : "a")} {organ.Value.ConvertCm()}",
            SexualOrganType.Vagina => $"{(capitalLeter ? "A" : "a")} {organ.Value.ConvertCm()} ",
            SexualOrganType.Anal => $"{(capitalLeter ? "A" : "a")} {organ.Value.ConvertCm()} long dick",
            _ => organ.Value.ConvertCm(),
        };
    }
}