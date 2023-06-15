using System;
using Character.EssenceStuff;
using Character.Organs.Fluids.SexualFluids;
using Safe_To_Share.Scripts.Character.Organs.SexualOrgan;
using UnityEngine;

namespace Character.Organs.OrgansContainers {
    [Serializable]
    public class AnalsContainer : OrgansContainer<Anal> {
        public AnalsContainer() : base(new Scat()) {
            list.Add(new Anal());
        }

        public override int GrowNewCostAt(int count) => 19 + Mathf.FloorToInt(Mathf.Pow(52f, count));

        public override int ReCycleOnce(ChangeLog changeLog) {
            if (list.Count > 1 && list.Find(a => a.BaseValue <= 1) is { } smallestBackDoor) {
                list.Remove(smallestBackDoor);
                return GrowNewCost;
            }

            return base.ReCycleOnce(changeLog);
        }

        public override void TickMin(int ticks = 1) {
            base.TickMin(ticks);
        }
    }
}