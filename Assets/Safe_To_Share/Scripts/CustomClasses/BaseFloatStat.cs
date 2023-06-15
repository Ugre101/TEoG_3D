using System;
using System.Linq;
using Character;
using Character.StatsStuff.Mods;
using UnityEngine;

namespace Safe_to_Share.Scripts.CustomClasses {
    [Serializable]
    public class BaseFloatStat : ITickHour {
        public delegate void StatChange();

        [SerializeField] float baseValue;
        [SerializeField] ModsContainer mods = new();
        bool dirty = true;
        float lastValue;
        public BaseFloatStat(float baseValue) => this.baseValue = baseValue;

        public virtual float BaseValue {
            get => baseValue;
            set {
                baseValue = value;
                Dirty = true;
            }
        }

        public virtual float Value => Dirty ? lastValue = CalcValue() : lastValue;

        protected virtual bool Dirty {
            get => dirty || Mods.Dirty;
            set {
                dirty = value;
                if (value)
                    StatDirtyEvent?.Invoke();
            }
        }

        public ModsContainer Mods => mods;

        public bool TickHour(int ticks = 1) => Mods.TickHour(ticks);
        public event StatChange StatDirtyEvent;

        protected virtual float CalcValue() {
            Dirty = false;
            Mods.Dirty = false;
            var percent = (100f + GetValue(ModType.Percent)) / 100f;

            return (BaseValue + GetValue(ModType.Flat)) * percent;

            float GetValue(ModType type) =>
                Mods.StatMods.Where(mod => mod.ModType == type).Sum(mod => mod.ModValue) +
                Mods.TempBaseStatMods.Where(mod => mod.ModType == type).Sum(mod => mod.ModValue);
        }
    }
}