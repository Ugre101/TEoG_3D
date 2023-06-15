using System;
using System.Linq;
using Character.StatsStuff.Mods;
using UnityEngine;

namespace Safe_to_Share.Scripts.CustomClasses {
    [Serializable]
    public class BaseConstIntStat {
        public delegate void StatChange();

        [SerializeField] ModsContainer mods = new();
        bool dirty = true;
        int lastValue;

        public BaseConstIntStat(int baseValue) => BaseValue = baseValue;

        public virtual int BaseValue { get; }

        public virtual int Value => Dirty ? lastValue = CalcValue() : lastValue;

        public virtual bool Dirty {
            get => dirty || Mods.Dirty;
            protected set {
                dirty = value;
                if (value)
                    StatDirtyEvent?.Invoke();
            }
        }

        public ModsContainer Mods => mods;
        public event StatChange StatDirtyEvent;

        protected virtual int CalcValue() {
            Dirty = false;
            Mods.Dirty = false;
            var percent = (100f + GetValue(ModType.Percent)) / 100f;

            return Mathf.RoundToInt((BaseValue + GetValue(ModType.Flat)) * percent);

            float GetValue(ModType type) =>
                Mods.StatMods.Where(mod => mod.ModType == type).Sum(mod => mod.ModValue) +
                Mods.TempBaseStatMods.Where(mod => mod.ModType == type).Sum(mod => mod.ModValue);
        }
    }
}