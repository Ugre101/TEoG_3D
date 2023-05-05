using System;
using Character.EssenceStuff;
using Character.Organs.Fluids.SexualFluids;
using Safe_To_Share.Scripts.Character.Organs.SexualOrgan;
using UnityEngine;

namespace Character.Organs.OrgansContainers
{
    [Serializable]
    public class BallsContainer : OrgansContainer<Balls>
    {
        public BallsContainer() : base(new Cum(), 0.5f)
        {
        }


        public override int GrowNewCostAt(int count) => 19 + Mathf.FloorToInt(Mathf.Pow(52f, count));

        public override void TickMin(int ticks = 1) => base.TickMin(ticks);

        public override int ReCycleOnce(ChangeLog changeLog)
        {
            if (list.Find(b => b.BaseValue == 1) is { } smallestBalls)
            {
                list.Remove(smallestBalls);
                changeLog.LogDrainChange("a pair of balls shrunk completely leaving a empty space");
                return GrowNewCost;
            }

            changeLog.LogDrainChange("a pair of balls shrunk");
            return base.ReCycleOnce(changeLog);
        }
    }
}