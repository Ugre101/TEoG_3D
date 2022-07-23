using System;
using Safe_to_Share.Scripts.CustomClasses;
using UnityEngine;

namespace Character.BodyStuff
{
    [Serializable]
    public class BodyStat : BaseFloatStat
    {
        public BodyStat(int baseValue) : base(baseValue)
        {
        }

        public override float BaseValue
        {
            get => base.BaseValue;
            set => base.BaseValue = Mathf.Max(0, value);
        }

        // Final value can't be under zero for body stats, e.g. you can't be -11cm long.
        protected override float CalcValue() => Mathf.Max(0, base.CalcValue());
    }
}