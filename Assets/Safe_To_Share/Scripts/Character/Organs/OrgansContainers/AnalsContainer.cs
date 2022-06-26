using System;
using Character.EssenceStuff;
using Character.Organs.Fluids.SexualFluids;
using UnityEngine;

namespace Character.Organs.OrgansContainers
{
    [Serializable]
    public class AnalsContainer : OrgansContainer
    {
        public AnalsContainer() : base(new Scat())
        {
        }

        public override int GrowNewCost => 19 + Mathf.FloorToInt(Mathf.Pow(52f, list.Count));

        public override int ReCycleOnce(ChangeLog changeLog)
        {
            if (list.Count > 1 && list.Find(a => a.BaseValue <= 1) is { } smallestBackDoor)
            {
                list.Remove(smallestBackDoor);
                return GrowNewCost;
            }

            return base.ReCycleOnce(changeLog);
        }
    }
}