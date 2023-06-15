using System;
using System.Collections.Generic;
using Character.StatsStuff.HealthStuff;
using UnityEngine;

namespace Character.StatsStuff {
    [Serializable]
    public class Stats : ITickMinute, ITickHour {
        [SerializeField] CharStat strength, intelligence, constitution, charisma, agility;

        [SerializeField] Health health, willPower;
        Dictionary<CharStatType, CharStat> getCharStats;


        public Stats(int strength, int intelligence, int constitution, int charisma, int agility) {
            this.strength = new CharStat(strength);
            this.intelligence = new CharStat(intelligence);
            this.constitution = new CharStat(constitution);
            this.charisma = new CharStat(charisma);
            this.agility = new CharStat(agility);
            health = new Health(50, new IntRecovery(1), new AffectByStat(Constitution, 5));
            willPower = new Health(50, new IntRecovery(1), new AffectByStat(Intelligence, 5));
        }

        public Stats() : this(10, 10, 10, 10, 10) { }

        public CharStat Strength => strength;

        public CharStat Intelligence => intelligence;

        public CharStat Constitution => constitution;

        public CharStat Charisma => charisma;

        public CharStat Agility => agility;
        public Health Health => health;

        public Health WillPower => willPower;
        public bool Dead => WillPower.Dead || Health.Dead;

        public Dictionary<CharStatType, CharStat> GetCharStats =>
            getCharStats ??= new Dictionary<CharStatType, CharStat> {
                { CharStatType.Strength, Strength },
                { CharStatType.Intelligence, Intelligence },
                { CharStatType.Constitution, Constitution },
                { CharStatType.Charisma, Charisma },
                { CharStatType.Agility, Agility },
            };

        // Dictionary<HealthTypes, BaseIntStat> getHealthBaseStats;

        // public Dictionary<HealthTypes, BaseIntStat> GetHealthBaseStats =>
        //     getHealthBaseStats ??= new Dictionary<HealthTypes, BaseIntStat>
        //     {
        //         {HealthTypes.Health, Health},
        //         {HealthTypes.HealthRecovery, Health.IntRecovery},
        //         {HealthTypes.WillPower, WillPower},
        //         {HealthTypes.WillPowerRecovery, WillPower.IntRecovery},
        //     };

        public bool TickHour(int ticks = 1) {
            var change = false;
            foreach (var stat in GetCharStats.Values)
                if (stat.Mods.TickHour(ticks))
                    change = true;
            if (Health.TickHour(ticks))
                change = true;
            if (WillPower.TickHour(ticks))
                change = true;
            return change;
        }

        public void TickMin(int ticks = 1) {
            Health.TickMin(ticks);
            WillPower.TickMin(ticks);
        }

        public void FullRecovery(int percent = 100) {
            Health.FullRecovery(percent);
            WillPower.FullRecovery(percent);
        }

        public void Loaded() {
            // ItemMods
            //  health = new Health(50, new IntRecovery(1), new AffectByStat(Constitution, 5));
            //   willPower = new Health(50, new IntRecovery(1), new AffectByStat(Intelligence, 5));

            WillPower.Refresh();
            Health.Refresh();

            // // Blessings & perks
            // void ApplyHealthStatMod(AssingHealthMod mod)
            // {
            //     if (GetHealthBaseStats.TryGetValue(mod.Type, out BaseIntStat baseStat))
            //         baseStat.Mods.AddStatMod(mod.Mod);
            // }
        }
    }
}