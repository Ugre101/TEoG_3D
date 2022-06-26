using System;
using Character.StatsStuff.Mods;
using UnityEngine;

namespace Character.BodyStuff
{
    [Serializable]
    public class AssignBodyMod : AssingIntMod
    {
        [SerializeField] BodyStatType type;

        public AssignBodyMod(IntMod mod, BodyStatType type) : base(mod) => this.type = type;

        public BodyStatType Type => type;
    }
}