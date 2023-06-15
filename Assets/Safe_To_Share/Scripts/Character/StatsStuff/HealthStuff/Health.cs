using System;
using System.Linq;

namespace Character.StatsStuff.HealthStuff {
    [Serializable]
    public class Health : RecoveryIntStat {
        readonly AffectByStat[] affectByStat;

        public Health(int baseValue, IntRecovery intRecovery, params AffectByStat[] affectByStat) : base(baseValue,
            intRecovery) {
            this.affectByStat = affectByStat;
            foreach (var stat in affectByStat)
                stat.affectedBy.StatDirtyEvent += SetDirty;
        }

        void SetDirty() {
            Dirty = true;
            InvokeMaxChange();
        }

        protected override int CalcValue() =>
            base.CalcValue() + affectByStat.Sum(stat => stat.effectAmount * stat.affectedBy.Value);
    }
}