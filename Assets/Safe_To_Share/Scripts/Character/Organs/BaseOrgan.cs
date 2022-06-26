using System;
using Character.EssenceStuff;
using Character.PregnancyStuff;
using Character.VoreStuff;
using CustomClasses;
using UnityEngine;

namespace Character.Organs
{
    [Serializable]
    public class BaseOrgan : BaseIntStat
    {
        [SerializeField] VoreOrgan voreOrgan = new();
        [SerializeField] Womb womb = new();

        public BaseOrgan() : base(1)
        {
        }

        public virtual int GrowCost => BaseCost;

        protected int BaseCost => 9 + Mathf.FloorToInt(Mathf.Pow(BaseValue, 1.2f));

        protected int HigherCost => 19 + Mathf.FloorToInt(Mathf.Pow(BaseValue, 1.23f));

        public VoreOrgan Vore => voreOrgan;

        public Womb Womb => womb;

        public bool Grow(Essence essence)
        {
            if (essence.Amount < GrowCost)
                return false;
            essence.Amount -= GrowCost;
            BaseValue++;
            return true;
        }

        public int Shrink()
        {
            BaseValue--;
            return GrowCost;
        }
    }
}