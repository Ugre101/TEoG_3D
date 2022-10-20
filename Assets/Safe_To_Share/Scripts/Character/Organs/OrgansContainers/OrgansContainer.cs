using System;
using System.Collections.Generic;
using System.Linq;
using Character.EssenceStuff;
using Character.Organs.Fluids;
using Character.Organs.Fluids.SexualFluids;
using Character.StatsStuff.Mods;
using UnityEngine;
using Random = System.Random;

namespace Character.Organs.OrgansContainers
{
    [Serializable]
    public abstract class  OrgansContainer : ITickMinute, ITickHour
    {
        [SerializeField] protected SexualFluid fluid;
        [SerializeField] protected List<BaseOrgan> list = new();
        protected OrgansContainer(FluidType fluidType, float startRec = 0f) =>
            fluid = new SexualFluid(fluidType, startRec);


        public int GrowNewCost => GrowNewCostAt(list.Count);
        public abstract int GrowNewCostAt(int amount);
        public string FluidType => Fluid.FluidType.Title;
        public int FluidCurrent => Mathf.RoundToInt(Fluid.CurrentValue / 100f * FluidMax);

        public int FluidMax => List.FluidMaxValueSphere(Fluid);

        public int Biggest => HaveAny() ? List.Max(o => o.Value) : 0;
        public IEnumerable<BaseOrgan> List => list;
        public SexualFluid Fluid => fluid;

        public bool TickHour(int ticks = 1)
        {
            Fluid.TickHour(ticks);
            bool change = false;
            foreach (BaseOrgan baseOrgan in List)
                if (baseOrgan.Mods.TickHour(ticks))
                    change = true;
            return change;
        }

        public virtual void TickMin(int ticks = 1) => Fluid.TickMin(ticks);

        public bool HaveAny() => List.Any();

        public BaseOrgan GetRandomOrgan()
        {
            Random rng = new();
            return list[rng.Next(list.Count)];
        }

        public bool RemoveOrgan(BaseOrgan organ) => list.Remove(organ);

        public bool TryGrowNew(Essence essence)
        {
            if (essence.Amount < GrowNewCost)
                return false;
            essence.LoseEssence(GrowNewCost);
            BaseOrgan newOrgan = new();
            if (list.Count > 0 && list[0].Mods.StatMods.Count > 0)
                // A bit of a hack but it should work
                foreach (IntMod modsStatMod in list[0].Mods.StatMods)
                    newOrgan.Mods.AddStatMod(modsStatMod);
            list.Add(newOrgan);
            return true;
        }

        public void GrowFirstAsMuchAsPossible(Essence essence)
        {
            BaseOrgan boobsOne = list.FirstOrDefault();
            if (boobsOne == null)
                return;
            while (boobsOne.Grow(essence))
            {
            }
        }

        public bool TryGrowSmallest(Essence essence)
            => HaveAny() && list.Aggregate((agg, next) => next.BaseValue > agg.BaseValue ? next : agg).Grow(essence);

        public virtual int ReCycleOnce(ChangeLog changeLog)
        {
            if (!HaveAny())
                return 0;
            return list.Count > 1
                ? List.Aggregate((agg, next) => next.BaseValue > agg.BaseValue ? next : agg).Shrink()
                : list[0].Shrink();
        }
    }
}