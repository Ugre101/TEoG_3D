using System;
using UnityEngine;

namespace Character.StatsStuff.Mods {
    [Serializable]
    public abstract class AssingIntMod {
        [SerializeField] IntMod mod;

        protected AssingIntMod(IntMod mod) => this.mod = mod;

        public IntMod Mod => mod;
    }
}