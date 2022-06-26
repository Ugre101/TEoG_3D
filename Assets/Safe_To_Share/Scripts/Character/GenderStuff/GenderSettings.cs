using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Character.GenderStuff
{
    public static class GenderSettings
    {
        static Dictionary<Gender, string> savedGenders;

        static Dictionary<Gender, string> SavedGenders => savedGenders ??= new Dictionary<Gender, string>
        {
            {Gender.Doll, PlayerPrefs.GetString(Gender.Doll.ToString(), "Doll")},
            {Gender.Male, PlayerPrefs.GetString(Gender.Male.ToString(), "Male")},
            {Gender.Female, PlayerPrefs.GetString(Gender.Female.ToString(), "Female")},
            {Gender.CuntBoy, PlayerPrefs.GetString(Gender.CuntBoy.ToString(), "Cunt boy")},
            {Gender.DickGirl, PlayerPrefs.GetString(Gender.DickGirl.ToString(), "Dick girl")},
            {Gender.Futanari, PlayerPrefs.GetString(Gender.Futanari.ToString(), "Futanari")},
            {Gender.MaleFutanari,PlayerPrefs.GetString(Gender.MaleFutanari.ToString(), "Futanari") }
        };

        public static void ReNameGender(Gender gender, string newName)
        {
            PlayerPrefs.SetString(gender.ToString(), newName);
            SavedGenders[gender] = newName;
        }

        public static string GenderString(this Gender gender, bool lowerCase = false)
            => SavedGenders.TryGetValue(gender, out string value) ? lowerCase ? value.ToLower() : value : "Err";

        public const int BoobThreesHold = 3;
        public static Gender GetGender(BaseCharacter baseCharacter)
        {
            bool hasBalls = baseCharacter.SexualOrgans.Balls.HaveAny();
            bool hasDick = baseCharacter.SexualOrgans.Dicks.HaveAny();
            bool hasVagina = baseCharacter.SexualOrgans.Vaginas.HaveAny();
            bool hasBoobs = baseCharacter.SexualOrgans.Boobs.Biggest > BoobThreesHold;
            if (hasDick)
            {
                if (hasVagina)
                    return hasBoobs ? Gender.Futanari : Gender.MaleFutanari;
                return hasBoobs ? Gender.DickGirl : Gender.Male;
            }
            if (hasVagina)
                return hasBoobs ? Gender.Female : Gender.CuntBoy;
            return Gender.Doll;
        }

        public static string HeShe(this Gender gender,bool capital = false)
        {
            string returnText;
            switch (gender)
            {
                case Gender.Doll:
                    returnText = "They";
                    break;
                case Gender.Male:
                case Gender.CuntBoy:
                case Gender.MaleFutanari:
                    returnText = "He";
                    break;
                case Gender.Female:
                case Gender.DickGirl:
                case Gender.Futanari:
                    returnText = "She";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gender), gender, null);
            }

            return capital ? returnText : returnText.ToLower();
        }
        public static string HisHer(this Gender gender,bool capital = false)
        {
            string returnText;
            switch (gender)
            {
                case Gender.Doll:
                    returnText = "Their";
                    break;
                case Gender.Male:
                case Gender.CuntBoy:
                case Gender.MaleFutanari:
                    returnText = "His";
                   break;
                case Gender.Female:
                case Gender.DickGirl:
                case Gender.Futanari:
                    returnText = "Her";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gender), gender, null);
            }

            return capital ? returnText : returnText.ToLower();
        }
        public static string HimHer(this Gender gender,bool capital = false)
        {
            string returnText;
            switch (gender)
            {
                case Gender.Doll:
                    returnText = "Them";
                    break;
                case Gender.Male:
                case Gender.CuntBoy:
                case Gender.MaleFutanari:
                    returnText = "Him";
                    break;
                case Gender.Female:
                case Gender.DickGirl:
                case Gender.Futanari:
                    returnText = "Her";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gender), gender, null);
            }

            return capital ? returnText : returnText.ToLower();
        }
    }
}