using System;
using Character;

namespace Safe_To_Share.Scripts.Character.Scat
{
    public static class ScatExtensions
    {
        public static string Status(this BaseCharacter character)
        {
            if (character.SexualOrgans.Anals.HaveAny())
            {
               int current =  character.SexualOrgans.Anals.FluidCurrent;
               int max = character.SexualOrgans.Anals.FluidMax;

            }
            return String.Empty;
        }
    }
}