using System;
using System.Collections.Generic;
using UnityEngine;

namespace Character.Organs.Fluids
{
    [Serializable]
    public class ForeignFluids
    {
        [SerializeField] List<ForeignFluid> fluids = new();

        public IEnumerable<ForeignFluid> GetFluids => fluids;

        public void AddFluid(string fluidType, float amount)
        {
            if (fluids.Exists(f => f.FluidType == fluidType))
                fluids.Find(f => f.FluidType == fluidType).Amount += amount;
            else
                fluids.Add(new ForeignFluid(fluidType, amount));
        }

        public void ClearFluids() => fluids.Clear();

        public void ClearFluidsByPercent(float percent)
        {
            foreach (ForeignFluid foreignFluid in fluids)
                foreignFluid.Amount *= (100f - percent) / 100f;
        }
        [Serializable]
        public class ForeignFluid
        {
            [SerializeField] float amount;
            [SerializeField] string fluidType;

            public ForeignFluid(string fluidType, float amount)
            {
                this.amount = amount;
                this.fluidType = fluidType;
            }

            public float Amount
            {
                get => amount;
                set => amount = value;
            }

            public string FluidType => fluidType;
        }
    }
}
