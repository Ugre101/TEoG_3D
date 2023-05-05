using System;
using Character.EssenceStuff;
using Character.Organs.Fluids.SexualFluids;
using Safe_To_Share.Scripts.Character.Organs.SexualOrgan;
using UnityEngine;

namespace Character.Organs.OrgansContainers
{
    [Serializable]
    public class VaginaContainer : OrgansContainer<Vagina>
    {
        public VaginaContainer() : base(new Femcum())
        {
        }

        public override int GrowNewCostAt(int count) => 19 + Mathf.FloorToInt(Mathf.Pow(52f, count));


        public override int ReCycleOnce(ChangeLog changeLog)
        {
            if (list.Find(v => v.BaseValue == 1) is { } smallestVag)
            {
                list.Remove(smallestVag);
                changeLog.LogDrainChange("a vagina tightened fully, closing the skin leaving no traces of it");
                return GrowNewCost;
            }

            changeLog.LogDrainChange("a vagina tightened");
            return base.ReCycleOnce(changeLog);
        }
    }
}