using System;
using System.Collections.Generic;
using System.Linq;
using Character.EssenceStuff;
using Character.Organs.Fluids.SexualFluids;
using UnityEngine;
using Random = System.Random;

namespace Character.Organs.OrgansContainers
{
    [Serializable]
    public abstract class OrgansContainer<TOrgan> : BaseOrgansContainer where TOrgan : BaseOrgan, new()
    {
        protected OrgansContainer(FluidType fluidType, float startRec = 0) : base(fluidType, startRec)
        {
        }
        [SerializeField] protected List<TOrgan> list = new();
        public override IEnumerable<BaseOrgan> BaseList => list;

        public override int OrgansCount => list.Count;

        public override BaseOrgan GetRandomOrgan()
        {
            Random rng = new();
            return list[rng.Next(list.Count)];
        }
        public void GrowFirstAsMuchAsPossible(Essence essence)
        {
            var boobsOne = list.FirstOrDefault();
            if (boobsOne == null)
                return;
            while (boobsOne.Grow(essence))
            {
            }
        }

        public override bool TryGrowNew(Essence essence)
        {
            if (essence.Amount < GrowNewCost)
                return false;
            essence.LoseEssence(GrowNewCost);
            TOrgan newOrgan = new();
            if (list.Count > 0 && list[0].Mods.StatMods.Count > 0) // A bit of a hack but it should work
                foreach (var modsStatMod in list[0].Mods.StatMods)
                    newOrgan.Mods.AddStatMod(modsStatMod);
            list.Add(newOrgan);
            return true;
        }
        public override bool RemoveOrgan(BaseOrgan organ)
        {
            if (organ is TOrgan removeMe) 
                return list.Remove(removeMe);
            return false;
        }

        public override bool TickHour(int ticks = 1)
        {
            {
                Fluid.TickHour(ticks);
                bool change = false;
                foreach (var baseOrgan in list)
                    if (baseOrgan.Mods.TickHour(ticks))
                        change = true;
                return change;
            }
        }
    }
}