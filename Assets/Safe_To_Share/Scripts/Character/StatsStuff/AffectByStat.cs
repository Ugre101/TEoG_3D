using System;

namespace Character.StatsStuff
{
    [Serializable]
    public class AffectByStat
    {
        public CharStat affectedBy;

        public int effectAmount;

        public AffectByStat(CharStat affectedBy, int effectAmount)
        {
            this.affectedBy = affectedBy;
            this.effectAmount = effectAmount;
        }
    }
}