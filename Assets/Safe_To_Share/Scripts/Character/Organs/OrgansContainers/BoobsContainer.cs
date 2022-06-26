using System;
using System.Collections.Generic;
using Character.EssenceStuff;
using Character.Organs.Fluids.SexualFluids;
using Character.StatsStuff.Mods;
using UnityEngine;

namespace Character.Organs.OrgansContainers
{
    [Serializable]
    public class BoobsContainer : OrgansContainer
    {
        public BoobsContainer() : base(new Milk())
        {
        }


        public override int GrowNewCost => 9 + Mathf.FloorToInt(Mathf.Pow(52f, list.Count));

        public override int ReCycleOnce(ChangeLog changeLog)
        {
            if (list.Count == 0)
                return 0;
            if(list.Find(b => b.BaseValue <= 0) is { } smallestBoob)
            {
                list.Remove(smallestBoob);
                changeLog.LogDrainChange("a pair of breasts shrunk completely leaving nothing behind");
                return GrowNewCost;
            }

            changeLog.LogDrainChange("a pair of breast lost some it's size");
            return base.ReCycleOnce(changeLog);
        }

        public void StartLactating() => Fluid.Recovery.Mods.StatMods.Add(new IntMod(1, "Pregnant", ModType.Flat));
    }
}