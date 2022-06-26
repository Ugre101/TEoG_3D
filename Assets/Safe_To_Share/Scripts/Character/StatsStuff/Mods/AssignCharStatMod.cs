using System;
using UnityEngine;

namespace Character.StatsStuff.Mods
{
    [Serializable]
    public class AssignCharStatMod : AssingIntMod
    {
        [SerializeField] CharStatType stat;

        public AssignCharStatMod(IntMod mod, CharStatType stat) : base(mod) => this.stat = stat;

        public CharStatType Stat => stat;
    }
}