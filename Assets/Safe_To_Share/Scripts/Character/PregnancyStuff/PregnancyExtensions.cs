using System.Collections.Generic;
using System.Linq;
using Character.Organs;
using UnityEngine;

namespace Character.PregnancyStuff {
    public static class PregnancyExtensions {
        public const int IncubationDays = 280;


        public static void TickPregnancy(this BaseCharacter mother, int ticks) {
            var growth = mother.PregnancySystem.PregnancySpeed.Value * ticks;
            List<Fetus> allBorn = new();
            foreach (var pair in mother.SexualOrgans.Containers)
            foreach (var baseOrgan in pair.Value.BaseList)
            foreach (var growFetuse in baseOrgan.Womb.GrowFetuses(growth))
                allBorn.Add(growFetuse);
            if (allBorn.Any())
                mother.OnBirth(allBorn);
        }

        public static bool TryImpregnate(this BaseCharacter father, BaseCharacter mother, BaseOrgan organ) {
            float chance = father.PregnancySystem.Virility.Value * mother.PregnancySystem.Fertility.Value;
            // 10 * 10 = 100, 100 / 10 000 = 0.01 % chance
            var roll = Random.value * 10000f;
            if (!(chance >= roll)) return false;
            mother.PregnancySystem.GotPregnant();
            father.PregnancySystem.DidImpregnate();
            organ.Womb.AddFetus(mother, father);
            var extra = chance - roll;
            var twinRoll = Random.value * 10000f;
            if (extra >= twinRoll)
                organ.Womb.AddFetus(mother, father);
            mother.GotPregnant();
            return true;
        }

        public static bool IsPregnant(this BaseCharacter character) {
            foreach (var con in character.SexualOrgans.Containers.Values)
            foreach (var org in con.BaseList)
                if (org.Womb.HasFetus)
                    return true;
            return false;
        }
    }
}