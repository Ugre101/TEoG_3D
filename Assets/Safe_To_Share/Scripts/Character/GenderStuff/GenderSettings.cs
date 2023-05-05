using System;
using System.Collections.Generic;
using UnityEngine;

namespace Character.GenderStuff
{
    public static class GenderSettings
    {
        const int BoobThreesHold = 3;
        static Dictionary<Gender, string> savedGenders;

        static Dictionary<Gender, string> SavedGenders => savedGenders ??= new Dictionary<Gender, string>
        {
            { Gender.Doll, PlayerPrefs.GetString(nameof(Gender.Doll), "Doll") },
            { Gender.Male, PlayerPrefs.GetString(nameof(Gender.Male), "Male") },
            { Gender.Female, PlayerPrefs.GetString(nameof(Gender.Female), "Female") },
            { Gender.CuntBoy, PlayerPrefs.GetString(nameof(Gender.CuntBoy), "Cunt boy") },
            { Gender.DickGirl, PlayerPrefs.GetString(nameof(Gender.DickGirl), "Dick girl") },
            { Gender.Futanari, PlayerPrefs.GetString(nameof(Gender.Futanari), "Futanari") },
            { Gender.MaleFutanari, PlayerPrefs.GetString(nameof(Gender.MaleFutanari), "Futanari") },
        };

        public static void ReNameGender(Gender gender, string newName)
        {
            PlayerPrefs.SetString(nameof(gender), newName);
            SavedGenders[gender] = newName;
        }

        public static string GenderString(this Gender gender, bool lowerCase = false)
            => SavedGenders.TryGetValue(gender, out var value) ? lowerCase ? value.ToLower() : value : "Err";

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

        public static string HeShe(this Gender gender, bool capital = false)
        {
            var returnText = gender switch
            {
                Gender.Doll => "They",
                Gender.Male => "He",
                Gender.CuntBoy => "He",
                Gender.MaleFutanari => "He",
                Gender.Female => "She",
                Gender.DickGirl => "She",
                Gender.Futanari => "She",
                _ => throw new ArgumentOutOfRangeException(nameof(gender), gender, null),
            };

            return capital ? returnText : returnText.ToLower();
        }

        public static string HisHer(this Gender gender, bool capital = false)
        {
            var returnText = gender switch
            {
                Gender.Doll => "Their",
                Gender.Male => "His",
                Gender.CuntBoy => "His",
                Gender.MaleFutanari => "His",
                Gender.Female => "Her",
                Gender.DickGirl => "Her",
                Gender.Futanari => "Her",
                _ => throw new ArgumentOutOfRangeException(nameof(gender), gender, null)
            };

            return capital ? returnText : returnText.ToLower();
        }

        public static string HimHer(this Gender gender, bool capital = false)
        {
            var returnText = gender switch
            {
                Gender.Doll => "Them",
                Gender.Male => "Him",
                Gender.CuntBoy => "Him",
                Gender.MaleFutanari => "Him",
                Gender.Female => "Her",
                Gender.DickGirl => "Her",
                Gender.Futanari => "Her",
                _ => throw new ArgumentOutOfRangeException(nameof(gender), gender, null)
            };

            return capital ? returnText : returnText.ToLower();
        }
    }
}