using System;
using UnityEngine;

namespace Character.StatsStuff.Mods {
    [Serializable]
    public class IntMod {
        [SerializeField] int modValue;
        [SerializeField] string from;
        [SerializeField] ModType modType;

        public IntMod(int modValue, string from, ModType modType) {
            this.modValue = modValue;
            this.from = from;
            this.modType = modType;
        }

        public int ModValue => modValue;

        public string From => from;

        public ModType ModType => modType;
    }
}