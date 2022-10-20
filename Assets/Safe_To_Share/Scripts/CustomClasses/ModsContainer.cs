using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Character.StatsStuff.Mods
{
    // Just to make accessing the stat cleaner
    [Serializable]
    public class ModsContainer : ITickHour
    {
        [SerializeField] List<TempIntMod> tempBaseStatMods = new();
        public bool Dirty { get; set; }

        public List<IntMod> StatMods { get; } = new();

        public List<TempIntMod> TempBaseStatMods => tempBaseStatMods;


        public bool TickHour(int ticks = 1)
        {
            Dirty = TempBaseStatMods.RemoveAll(mod => mod.TickDown(ticks)) > 0 || Dirty;
            return Dirty;
        }

        public void AddStatMod(IntMod mod)
        {
            StatMods.Add(mod);
            Dirty = true;
        }

        public bool HaveModFrom(string from) => StatMods.Exists(m => m.From == from);


        public void RemoveStatMod(IntMod mod) => Dirty = StatMods.Remove(mod);

        public bool 
            RemoveStatModsFromSource(string source)
        {
            Dirty = StatMods.RemoveAll(mod => mod.From.Equals(source)) > 0;
            return Dirty;
        }

        public void AddTempStatMod(TempIntMod mod)
        {
            if (TempBaseStatMods.Exists(m => m.From == mod.From && m.ModType == mod.ModType))
            {
                TempIntMod current = TempBaseStatMods.Find(m => m.From == mod.From && m.ModType == mod.ModType);
                int progressiveLess = mod.HoursLeft - current.HoursLeft / 2;
                if (progressiveLess > 0)
                    current.AddHours(progressiveLess);
            }
            else
            {
                TempBaseStatMods.Add(mod);
                Dirty = true;
            }
        }

        public float GetValueOfType(ModType type) =>
            StatMods.Where(mod => mod.ModType == type).Sum(mod => mod.ModValue) +
            TempBaseStatMods.Where(mod => mod.ModType == type).Sum(mod => mod.ModValue);

        public void AddTempStatMod(int duration, int value, string from, ModType type)
            => AddTempStatMod(new TempIntMod(duration, value, from, type));

        // if false do not reset dirty
        public void RemoveTempStatMod(TempIntMod mod) => Dirty = TempBaseStatMods.Remove(mod) || Dirty;

        public void RemoveTempStatModsFromSource(string source) => Dirty =
            TempBaseStatMods.RemoveAll(mod => mod.From.Equals(source)) > 0 || Dirty;
    }
}