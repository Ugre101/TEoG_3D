using System;
using Character.StatsStuff;
using CustomClasses;
using Safe_to_Share.Scripts.CustomClasses;

namespace Character.EssenceStuff
{
    [Serializable]
    public class StableEssence : BaseIntStat
    {
        public StableEssence(int baseValue) : base(baseValue)
        {
        }
    }
}