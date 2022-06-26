using System;
using Character.StatsStuff;
using Safe_to_Share.Scripts.CustomClasses;

namespace Character.Organs.Fluids
{
    [Serializable]
    public class FloatRecovery : BaseFloatStat
    {
        public FloatRecovery(float baseValue) : base(baseValue)
        {
        }
    }
}