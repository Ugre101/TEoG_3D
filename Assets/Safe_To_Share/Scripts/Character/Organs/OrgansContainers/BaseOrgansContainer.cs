using System;
using System.Collections.Generic;
using System.Linq;
using Character.EssenceStuff;
using Character.Organs.Fluids;
using Character.Organs.Fluids.SexualFluids;
using UnityEngine;

namespace Character.Organs.OrgansContainers {
    [Serializable]
    public abstract class BaseOrgansContainer : ITickMinute, ITickHour {
        [SerializeField] protected SexualFluid fluid;

        //[SerializeField] protected List<BaseOrgan> list = new();
        protected BaseOrgansContainer(FluidType fluidType, float startRec = 0f) =>
            fluid = new SexualFluid(fluidType, startRec);


        public abstract int OrgansCount { get; }
        public int GrowNewCost => GrowNewCostAt(OrgansCount);
        public string FluidType => Fluid.FluidType.Title;
        public int FluidCurrent => Mathf.RoundToInt(Fluid.CurrentValue / 100f * FluidMax);

        public int FluidMax => BaseList.FluidMaxValueSphere(Fluid);

        public int Biggest => HaveAny() ? BaseList.Max(o => o.Value) : 0;
        public abstract IEnumerable<BaseOrgan> BaseList { get; }
        public SexualFluid Fluid => fluid;

        public abstract bool TickHour(int ticks = 1);


        public virtual void TickMin(int ticks = 1) => Fluid.TickMin(ticks);
        public abstract int GrowNewCostAt(int amount);

        public bool HaveAny() => BaseList.Any();

        public abstract BaseOrgan GetRandomOrgan();


        public abstract bool TryGrowNew(Essence essence);


        public bool TryGrowSmallest(Essence essence) =>
            HaveAny() && BaseList.Aggregate((agg, next) => next.BaseValue > agg.BaseValue ? next : agg).Grow(essence);

        public abstract bool RemoveOrgan(BaseOrgan baseOrgan);

        public virtual int ReCycleOnce(ChangeLog changeLog) {
            if (!HaveAny())
                return 0;
            return OrgansCount > 1
                ? BaseList.Aggregate((agg, next) => next.BaseValue > agg.BaseValue ? next : agg).Shrink()
                : BaseList.First().Shrink();
        }
    }
}