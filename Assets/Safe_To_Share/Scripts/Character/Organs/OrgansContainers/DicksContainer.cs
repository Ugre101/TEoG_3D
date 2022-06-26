using System;
using Character.EssenceStuff;
using Character.Organs.Fluids.SexualFluids;
using UnityEngine;

namespace Character.Organs.OrgansContainers
{
    [Serializable]
    public class DicksContainer : OrgansContainer
    {
        public DicksContainer() : base(new Cum())
        {
        }

        public override int GrowNewCost => 9 + Mathf.FloorToInt(Mathf.Pow(52f, list.Count));

        public override int ReCycleOnce(ChangeLog changeLog)
        {
            if (list.Find(d => d.BaseValue <= 1) is { } littleDick)
            {
                changeLog.LogDrainChange("a dick shrunk complete leaving nothing behind");
                list.Remove(littleDick);
                return GrowNewCost;
            }
            changeLog.LogDrainChange("a dick shrunk");
            return base.ReCycleOnce(changeLog);
        }
    }
}