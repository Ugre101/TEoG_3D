using System;
using Safe_to_Share.Scripts.CustomClasses;

namespace Character.StatsStuff.HealthStuff {
    [Serializable]
    public class IntRecovery : BaseConstIntStat {
        public IntRecovery(int baseValue) : base(baseValue) { }
    }
}