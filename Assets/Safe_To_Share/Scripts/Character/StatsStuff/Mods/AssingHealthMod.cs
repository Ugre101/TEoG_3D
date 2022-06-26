using System;
using Character.StatsStuff.HealthStuff;
using UnityEngine;

namespace Character.StatsStuff.Mods
{
    [Serializable]
    public class AssingHealthMod : AssingIntMod
    {
        [SerializeField] HealthTypes type;

        public AssingHealthMod(IntMod mod, HealthTypes type) : base(mod) => this.type = type;

        public HealthTypes Type => type;
    }
}