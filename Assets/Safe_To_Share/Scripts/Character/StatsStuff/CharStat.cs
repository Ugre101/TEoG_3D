using System;
using CustomClasses;

namespace Character.StatsStuff {
    [Serializable]
    public class CharStat : BaseIntStat {
        public CharStat(int baseValue) : base(baseValue) { }
    }
}