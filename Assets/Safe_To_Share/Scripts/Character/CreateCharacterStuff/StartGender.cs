using System;
using System.Collections.Generic;
using System.Linq;
using Character.EssenceStuff;
using Character.GenderStuff;
using Character.IslandData;
using UnityEngine;
using Random = System.Random;

namespace Character.CreateCharacterStuff
{
    [Serializable]
    public struct GenderBias
    {
        public int weight;
        public Gender gender;

        public GenderBias(int weight, Gender gender)
        {
            this.weight = weight;
            this.gender = gender;
        }
    }

    [Serializable]
    public class StartGender
    {
        static Random rng = new();

        [SerializeField] GenderBias[] allowedGenders =
        {
            new(3, Gender.Male), new(3, Gender.Female),
            new(2, Gender.Futanari), new(1, Gender.DickGirl),
            new(1, Gender.CuntBoy), new(0, Gender.Doll),
        };

        [SerializeField] int startEssence;

        // 0 => 0.3 Male
        // 0.4 DGirl
        // 0.5 => 0.7 Futa
        // 0.7 CBoy
        // 0.8 => 1 Female

        public void SetGender(BaseCharacter character)
        {
            EssenceSystem essence = character.Essence;
            switch (GetGender())
            {
                case Gender.Doll:
                    essence.StableEssence.BaseValue +=
                        Mathf.RoundToInt(startEssence / 1.9f); // 1.9f for a little head space
                    essence.Femininity.GainEssence(startEssence / 2);
                    essence.Masculinity.GainEssence(startEssence / 2);
                    break;
                case Gender.Male:
                    essence.Masculinity.GainEssence(startEssence);
                    break;
                case Gender.Female:
                    essence.Femininity.GainEssence(startEssence);
                    break;
                case Gender.CuntBoy:
                    Essence tempEss = new();
                    tempEss.GainEssence( startEssence);
                    if (character.SexualOrgans.Vaginas.TryGrowNew(tempEss))
                    {
                        character.SexualOrgans.Vaginas.GrowFirstAsMuchAsPossible(tempEss);
                        essence.StableEssence.BaseValue +=
                            Mathf.RoundToInt(tempEss.Amount * 1.1f); // Whats left plus a small head space
                        essence.Masculinity.GainEssence(tempEss.Amount / 2);
                        essence.Femininity.GainEssence(tempEss.Amount / 2);
                    }

                    break;
                case Gender.DickGirl:
                    essence.Masculinity.GainEssence( startEssence / 2);
                    character.GrowOrgans(); // Grow male stuff before stable essence if added
                    Essence dickGirlEssence = new();
                    dickGirlEssence.GainEssence(startEssence / 2);
                    if (character.SexualOrgans.Boobs.HaveAny() ||
                        character.SexualOrgans.Boobs.TryGrowNew(dickGirlEssence))
                    {
                        character.SexualOrgans.Boobs.GrowFirstAsMuchAsPossible(dickGirlEssence);
                        essence.StableEssence.BaseValue +=
                            Mathf.RoundToInt(dickGirlEssence.Amount * 1.1f); // Whats left plus a small head space
                        essence.Masculinity.GainEssence( dickGirlEssence.Amount / 2);
                        essence.Femininity.GainEssence( dickGirlEssence.Amount / 2);
                    }

                    break;
                case Gender.MaleFutanari:
                    essence.Femininity.GainEssence(startEssence / 2);
                    if (character.SexualOrgans.Vaginas.TryGrowNew(essence.Femininity))
                    {
                        character.SexualOrgans.Vaginas.GrowFirstAsMuchAsPossible(essence.Femininity);
                        essence.StableEssence.BaseValue +=
                            Mathf.RoundToInt(essence.Femininity.Amount * 1.1f); // Whats left plus a small head space
                    }

                    essence.Masculinity.GainEssence( startEssence / 2);
                    break;
                case Gender.Futanari:
                default:
                    essence.Femininity.GainEssence(startEssence / 2);
                    essence.Masculinity.GainEssence( startEssence / 2);
                    break;
            }

            character.GrowOrgans();
        }

        Gender GetGender()
        {
            if (allowedGenders.Length < 1) return Gender.Doll; // TODO Add safe essence
            if (allowedGenders.Length == 1) return allowedGenders.FirstOrDefault().gender;

            int totalWeight = allowedGenders.Sum(g => g.weight);
            List<GenderBias> weightedList = new();
            int currWeight = 0;
            foreach (GenderBias bias in allowedGenders)
            {
                currWeight += bias.weight;
                weightedList.Add(new GenderBias(currWeight, bias.gender));
            }

            int index = rng.Next(totalWeight);
            foreach (GenderBias bias in weightedList.Where(bias => bias.weight > index))
                return bias.gender;
            return Gender.Futanari;
        }

        public void SetGender(BaseCharacter character, Islands islands)
        {
            SetGender(character);
            if (!IslandStonesDatas.IslandDataDict.TryGetValue(islands, out var data)) return;
            int masc = data.essenceData.Masc.Current;
            if (masc > 0)
                character.GainMasc(masc);
            else if (masc < 0)
                character.LoseMasc(masc, new ChangeLog());
            int femi = data.essenceData.Femi.Current;
            if (femi > 0)
                character.GainFemi(femi);
            else if (femi < 0)
                character.LoseFemi(femi, new ChangeLog());
        }
    }
}