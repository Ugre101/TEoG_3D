using System;
using CustomClasses;
using Safe_to_Share.Scripts.CustomClasses;

namespace Character.StatsStuff
{
    [Serializable]
    public class CharStat : BaseIntStat
    {
        public CharStat(int baseValue) : base(baseValue)
        {
        }
    }
}