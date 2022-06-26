using System;

namespace Character.StatsStuff
{
    [Serializable]
    public struct RequireCharStat
    {
        public CharStatType StatType;
        public int Amount;
    }
}