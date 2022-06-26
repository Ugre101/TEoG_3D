using System.Collections.Generic;
using Character.Organs;
using Character.Organs.OrgansContainers;
using UnityEngine;

namespace Character.PregnancyStuff
{
    public static class PregnancyExtensions
    {
        public const int IncubationDays = 280;


        public static void TickPregnancy(this BaseCharacter mother, int ticks)
        {
            int growth = mother.PregnancySystem.PregnancySpeed.Value * ticks;
            foreach (KeyValuePair<SexualOrganType, OrgansContainer> pair in mother.SexualOrgans.Containers)
            foreach (BaseOrgan baseOrgan in pair.Value.List)
                baseOrgan.Womb.GrowFetuses(mother.OnBirth, growth);
        }

        public static bool TryImpregnate(this BaseCharacter father, BaseCharacter mother, BaseOrgan organ)
        {
            float chance = father.PregnancySystem.Virility.Value * mother.PregnancySystem.Fertility.Value;
            // 10 * 10 = 100, 100 / 10 000 = 0.01 % chance
            float roll = Random.value * 10000f;
            if (chance >= roll)
            {
                mother.PregnancySystem.GotPregnant();
                father.PregnancySystem.DidImpregnate();
                organ.Womb.AddFetus(mother, father);
                float extra = chance - roll;
                float twinRoll = Random.value * 10000f;
                if (extra >= twinRoll)
                    organ.Womb.AddFetus(mother, father);
                mother.GotPregnant();
                return true;
            }

            return false;
        }

        public static bool IsPregnant(this BaseCharacter character)
        {
            foreach (var con in character.SexualOrgans.Containers.Values)
                foreach (var org in con.List)
                    if (org.Womb.HasFetus)
                        return true;
            return false;
        }
    }
}