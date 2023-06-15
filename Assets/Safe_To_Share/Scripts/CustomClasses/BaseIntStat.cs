using System;
using System.Linq;
using Character.StatsStuff.Mods;
using UnityEngine;

namespace CustomClasses {
    [Serializable]
    public class BaseIntStat {
        public delegate void StatChange();

        [SerializeField] int baseValue;
        [SerializeField] ModsContainer mods = new();
        bool dirty = true;
        int lastValue;
        public BaseIntStat(int baseValue) => this.baseValue = baseValue;

        public virtual int BaseValue {
            get => baseValue;
            set {
                baseValue = value;
                Dirty = true;
            }
        }

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
            var percent = (100 + GetValue(ModType.Percent)) / 100;

            return Mathf.RoundToInt((BaseValue + GetValue(ModType.Flat)) * percent);

            float GetValue(ModType type) =>
                Mods.StatMods.Where(mod => mod.ModType == type).Sum(mod => mod.ModValue) +
                Mods.TempBaseStatMods.Where(mod => mod.ModType == type).Sum(mod => mod.ModValue);
        }
    }
}