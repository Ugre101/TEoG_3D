using System;
using Character;

namespace Safe_To_Share.Scripts.Character.Scat
{
    public static class ScatExtensions
    {
        public const float NeedToShitThreesHold = 0.4f;
        public const float NeedToPissThreesHold = 0.4f;
        static NeedToShit needToShit = new();
        public static string CheckBodyNeeds(this BaseCharacter character)
        {
            if (character.SexualOrgans.Anals.HaveAny())
            {
               int current =  character.SexualOrgans.Anals.FluidCurrent;
               int max = character.SexualOrgans.Anals.FluidMax;

               float percent = (float)current / max;
               if (percent < 0.3f)
               {
                   // "Your bowels are empty."
                   // empty bowels
               }else if (percent < 0.6f)
               {
                   // "There is some movement in your bowels"
                   // starting to build up
               }
               else if (percent < 0.8f)
               {
                   // you should start seeking for a good place
                   needToShit.Gain(character);
               }
               else if (percent < 0.9f)
               {
                   needToShit.Cure(character);
                   // presure on your ring
               }
               else
               {
                   // you feel like you are about to sh** yourself
               }
               

            }

            // var pressure = character.BodyFunctions.Bladder
            return String.Empty;
        }
    }
}