using System;
using UnityEngine;

namespace Character.StatsStuff.Mods
{
    [Serializable]
    public class TempIntMod : IntMod
    {
        [SerializeField] int hoursLeft;

        public TempIntMod(int hoursLeft, int modValue, string from, ModType modType) : base(modValue, from, modType) =>
            this.hoursLeft = hoursLeft;

        public int HoursLeft => hoursLeft;
        public void AddHours(int hours) => hoursLeft += hours;
        public bool TickDown(int tick = 1)
        {
            hoursLeft -= tick;
            return hoursLeft < 1;
        }
    }
}