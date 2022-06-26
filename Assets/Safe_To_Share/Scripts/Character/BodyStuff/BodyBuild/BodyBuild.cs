using System;
using Safe_to_Share.Scripts.CustomClasses;
using UnityEngine;

namespace Character.BodyStuff.BodyBuild
{
    [Serializable]
    public abstract class BodyBuild : BaseFloatStat
    {
        protected BodyBuild(float baseValue) : base(baseValue)
        {
        }

        public override float BaseValue { get => base.BaseValue; set => base.BaseValue = Mathf.Clamp(value, MinValue,MaxValue); }

        protected abstract float MinValue { get; }
        protected abstract float MaxValue { get; }

        protected abstract string Desc();
    }
}