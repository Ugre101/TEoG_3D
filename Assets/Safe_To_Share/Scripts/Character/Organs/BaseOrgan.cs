using System;
using Character.EssenceStuff;
using Character.PregnancyStuff;
using Character.VoreStuff;
using CustomClasses;
using UnityEngine;

namespace Character.Organs {
    [Serializable]
    public abstract class BaseOrgan : BaseIntStat {
        [SerializeField] VoreOrgan voreOrgan = new();
        [SerializeField] Womb womb = new();

        public BaseOrgan() : base(1) { }

        public virtual int GrowCost => GrowCostAt(BaseValue);

        public VoreOrgan Vore => voreOrgan;

        public Womb Womb => womb;

        public abstract string OrganDesc(bool capitalLeter = true);
        public virtual int GrowCostAt(int size) => 9 + Mathf.FloorToInt(Mathf.Pow(size, 1.2f));

        public bool Grow(Essence essence) {
            if (essence.Amount < GrowCost)
                return false;
            essence.LoseEssence(GrowCost);
            BaseValue++;
            return true;
        }

        public int Shrink() {
            BaseValue--;
            return GrowCost;
        }
    }
}