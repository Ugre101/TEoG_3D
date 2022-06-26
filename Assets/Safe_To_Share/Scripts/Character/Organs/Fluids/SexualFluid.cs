using System;
using Character.Organs.Fluids.SexualFluids;
using Character.StatsStuff.HealthStuff;
using UnityEngine;

namespace Character.Organs.Fluids
{
    [Serializable]
    public class SexualFluid : RecoveryFloatStat
    {
        [SerializeField] string fluidName;

        public SexualFluid(FluidType fluid, float startRec = 0) : base(100, new FloatRecovery(startRec)) =>
            fluidName = fluid.GetType().Name;

        public FluidType FluidType => FluidTypes.GetFluid(fluidName);

        public void ChangeFluidType(FluidType newFluid) => fluidName = newFluid.GetType().Name;
    }
}