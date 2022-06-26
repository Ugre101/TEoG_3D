using System;
using System.Linq;
using Character;
using Character.StatsStuff.Mods;
using UnityEngine;

namespace Safe_to_Share.Scripts.CustomClasses
{
    [Serializable]
    public class BaseConstFloatStat : ITickHour
    {
        public delegate void StatChange();

        [SerializeField] ModsContainer mods = new();
        bool dirty = true;
        float lastValue;
        public BaseConstFloatStat(float baseValue) => BaseValue = baseValue;

        public virtual float BaseValue { get; }

        public virtual float Value => Dirty ? lastValue = CalcValue() : lastValue;

        public virtual bool Dirty
        {
            get => dirty || Mods.Dirty;
            protected set
            {
                dirty = value;
                if (value)
                    StatDirtyEvent?.Invoke();
            }
        }

        public ModsContainer Mods => mods;

        public bool TickHour(int ticks = 1) => Mods.TickHour(ticks);
        public event StatChange StatDirtyEvent;

        protected virtual float CalcValue()
        {
            Dirty = false;
            Mods.Dirty = false;
            float percent = (100f + GetValue(ModType.Percent)) / 100f;

            return (BaseValue + GetValue(ModType.Flat)) * percent;

            float GetValue(ModType type) =>
                Mods.StatMods.Where(mod => mod.ModType == type).Sum(mod => mod.ModValue) +
                Mods.TempBaseStatMods.Where(mod => mod.ModType == type).Sum(mod => mod.ModValue);
        }
    }
}