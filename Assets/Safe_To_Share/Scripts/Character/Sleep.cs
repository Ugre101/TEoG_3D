using Character.PlayerStuff;
using Character.StatsStuff;
using Character.StatsStuff.Mods;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Character
{
    public static class SleepExtensions
    {
        const int SleepCooldown = 12;
        public const string ModFrom = "Sleep";

        public static bool PlayerCanSleep(this Player player) =>
            DateSystem.DateSaveHoursAgo(player.LastTimeSlept) >= SleepCooldown;

        public static int HoursBeforeCanSleep(Player player) =>
            Mathf.Max(-1, SleepCooldown - DateSystem.DateSaveHoursAgo(player.LastTimeSlept));

        public static void Sleep(this Player player, int sleepQuality, BaseCharacter[] sleepEnemies)
        {
            if (SleepAttack(player, sleepEnemies))
                return;
            Sleep(player, sleepQuality);
        }

        public static void Sleep(this Player player, int sleepQuality)
        {
            player.BaseSleep(sleepQuality);
            player.LastTimeSlept = DateSystem.Save();
            player.CheckAilments();
        }

        static bool SleepAttack(Player player, BaseCharacter[] sleepEnemies)
        {
            if (sleepEnemies == null)
                return false;
            int attackChance = 30; // sub perk values
            if (Random.Range(0, 100) >= attackChance)
                return false;
            TerribleSleep(player);
            // player.TriggerCombat(sleepEnemies[UnityEngine.Random.Range(0, sleepEnemies.Length)]);
            return true;
        }

        public static void BaseSleep(this BaseCharacter character, int sleepQuality)
        {
            int roll = Random.Range(0, 100);
            roll += sleepQuality;
            if (roll < 100)
                BadSleep(character);
            else if (roll <= 200)
                BasicSleep(character);
            else if (roll <= 300)
                GoodSleep(character);
            else
                GreatSleep(character);
        }


        static void SleepMods(BaseCharacter character, int healthPercent, int recFlat, int statFlat, int orgFlat = 0)
        {
            character.Stats.Health.Mods.AddTempStatMod(8, healthPercent, ModFrom, ModType.Percent);
            character.Stats.Health.IntRecovery.Mods.AddTempStatMod(8, recFlat, ModFrom, ModType.Flat);

            character.Stats.WillPower.Mods.AddTempStatMod(8, healthPercent, ModFrom, ModType.Percent);
            character.Stats.WillPower.IntRecovery.Mods.AddTempStatMod(8, recFlat, ModFrom, ModType.Flat);
            foreach (CharStat charStat in character.Stats.GetCharStats.Values)
                charStat.Mods.AddTempStatMod(8, statFlat, ModFrom, ModType.Flat);
            if (orgFlat != 0)
                character.SexStats.MaxCasterOrgasms.Mods.AddTempStatMod(8, orgFlat, ModFrom, ModType.Flat);
        }

        static void TerribleSleep(BaseCharacter character)
        {
            DateSystem.PassHour(3);
            SleepMods(character, -30, -2, -4, -1);
        }

        static void BadSleep(BaseCharacter baseCharacter)
        {
            DateSystem.PassHour(5);
            SleepMods(baseCharacter, -10, -1, -2);
        }

        static void BasicSleep(BaseCharacter baseCharacter)
        {
            DateSystem.PassHour(8);
            SleepMods(baseCharacter, 5, 1, 1);
        }

        static void GoodSleep(BaseCharacter baseCharacter)
        {
            DateSystem.PassHour(8);
            SleepMods(baseCharacter, 15, 2, 3, 1);
        }

        static void GreatSleep(BaseCharacter character)
        {
            DateSystem.PassHour(10);
            SleepMods(character, 30, 3, 5, 2);
        }

        public static string SleepTitle(float hpPer) =>
            hpPer switch
            {
                -30 => "Slept terrible",
                -10 => "Slept bad",
                5 => "Slept",
                15 => "Slept good",
                30 => "Slept great",
                _ => "Slept",
            };
    }
}