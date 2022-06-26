using System;
using CustomClasses;
using Safe_to_Share.Scripts.CustomClasses;
using UnityEngine;

namespace Character.PregnancyStuff
{
    [Serializable]
    public class PregnancySystem : ITickHour
    {
        [SerializeField] BaseIntStat fertility = new(35);
        [SerializeField] BaseIntStat virility = new(35);
        [SerializeField] BaseConstIntStat pregnancySpeed = new(1);
        [SerializeField] int timesPregnant, timesImpregnated;

        public BaseConstIntStat PregnancySpeed => pregnancySpeed;

        public BaseIntStat Fertility => fertility;

        public BaseIntStat Virility => virility;

        public int TimesPregnant => timesPregnant;

        public int TimesImpregnated => timesImpregnated;

        public bool TickHour(int ticks = 1)
        {
            bool change = Fertility.Mods.TickHour(ticks);
            if (Virility.Mods.TickHour(ticks))
                change = true;
            if (PregnancySpeed.Mods.TickHour(ticks))
                change = true;
            return change;
        }

        public void DidImpregnate(int value = 1) => timesImpregnated += value;

        public void GotPregnant(int value = 1) => timesPregnant += value;
    }
}