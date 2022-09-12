using System;
using System.Collections.Generic;
using System.Linq;
using Character.LevelStuff;
using Character.Organs;
using UnityEngine;

namespace Character.EssenceStuff
{
    public static class EssenceExtensions
    {
        const int MaxLoop = 99;

        public static void GainPerk(this EssencePerk perk, BaseCharacter gainer)
        {
            if (perk == null)
            {
                Debug.LogError("Perk is null");
                return;
            }

            if (gainer.Essence.EssencePerks.Contains(perk))
            {
                Debug.LogWarning("Tried to gain already owned perk");
                return;
            }

            perk.PerkGainedEffect(gainer);
            gainer.Essence.EssencePerks.Add(perk);
        }

        public static int DrainAmount(this BaseCharacter drainer, BaseCharacter loser)
        {
            int loserFlat = loser.Essence.EssencePerks.Sum(ep => ep.LoseFlatBonus);
            float loserPercent = 1f + Mathf.Round(loser.Essence.EssencePerks.Sum(ep => ep.LostPercentBonus) / 100f);
            int loserAmount = Mathf.RoundToInt(drainer.Essence.DrainAmount.Value * loserPercent) + loserFlat;
            return loserAmount;
        }

        public static ChangeLog DrainEssenceOfType(this BaseCharacter drainer, BaseCharacter loser,
            DrainEssenceType ofType, int bonusAmount = 0) => ofType switch
            {
                DrainEssenceType.None => new(),
                DrainEssenceType.Masc => drainer.DrainMasc(loser, bonusAmount),
                DrainEssenceType.Femi => drainer.DrainFemi(loser, bonusAmount),
                DrainEssenceType.Both => drainer.DrainBoth(loser, bonusAmount),
                _ => throw new ArgumentOutOfRangeException()
            };

        public static ChangeLog DrainBoth(this BaseCharacter drainer, BaseCharacter loser, int bonusAmount = 0)
        {
            ChangeLog changeLog = new();
            int drainAmount = (drainer.DrainAmount(loser) + bonusAmount) / 2;
            if (loser.CanDrainFemi() && loser.CanDrainMasc())
            {
                drainer.GainFemi(loser.LoseFemi(drainAmount / 2, changeLog));
                drainer.GainMasc(loser.LoseMasc(drainAmount / 2, changeLog));
            }
            else if (loser.CanDrainFemi())
                drainer.GainFemi(loser.LoseFemi(drainAmount, changeLog));
            else if (loser.CanDrainMasc())
                drainer.GainMasc(loser.LoseMasc(drainAmount, changeLog));

            drainer.GainMasc(loser.LoseMasc(drainAmount, changeLog));
            drainer.GainFemi(loser.LoseFemi(drainAmount, changeLog));
            foreach (EssencePerk perk in drainer.Essence.EssencePerks)
                perk.PerkDrainEssenceEffect(drainer, loser);
            foreach (EssencePerk perk in loser.Essence.EssencePerks)
                perk.PerkGetDrainedEssenceEffect(loser, drainer);
            return changeLog; // TODO fix
        }

        public static Essence GetEssenceOfType(this BaseCharacter character, EssenceType essenceType) => essenceType switch
            {
                EssenceType.Masc => character.Essence.Masculinity,
                EssenceType.Femi => character.Essence.Femininity,
                _ => throw new ArgumentOutOfRangeException(nameof(essenceType), essenceType, null)
            };

        public static int CalcTotalEssenceOfType(this BaseCharacter character, EssenceType essenceType) => essenceType switch
            {
                EssenceType.Masc => CalcTotalMasc(character),
                EssenceType.Femi => CalcTotalFemi(character),
                _ => throw new ArgumentOutOfRangeException(nameof(essenceType), essenceType, null)
            };

        #region Masc

        public static int CalcTotalMasc(this BaseCharacter ofCharacter)
        {
            int tot = ofCharacter.Essence.Masculinity.Amount;
            tot += ofCharacter.SexualOrgans.Balls.TotalEssenceCost();
            tot += ofCharacter.SexualOrgans.Dicks.TotalEssenceCost();
            return tot;
        }
        public static void GainMasc(this BaseCharacter gainer, int toGain)
        {
            gainer.Essence.Masculinity.GainEssence(toGain);
            gainer.GrowOrgans();
            gainer.Essence.Masculinity.InvokeEssenceChange();
        }

        public static int LoseMasc(this BaseCharacter loser, int amountToLose, ChangeLog changeLog)
        {
            int amountCollected = loser.Essence.Masculinity.LoseEssence(amountToLose);
            if (amountCollected < amountToLose)
            {
                int dickSum = loser.SexualOrgans.Dicks.List.Sum(d => d.BaseValue);
                int ballsSum = loser.SexualOrgans.Balls.List.Sum(b => b.BaseValue);
                const float dickDiv = 0.7f;
                int breakOut = 0;
                while (amountCollected < amountToLose && (dickSum > 0 || ballsSum > 0))
                {
                    if (MaxLoop < breakOut) break;
                    breakOut++;
                    if (dickSum > 0 && dickSum * dickDiv > ballsSum)
                    {
                        amountCollected += loser.SexualOrgans.Dicks.ReCycleOnce(changeLog);
                        dickSum = loser.SexualOrgans.Dicks.List.Sum(d => d.BaseValue);
                    }
                    else if (ballsSum > 0)
                    {
                        amountCollected += loser.SexualOrgans.Balls.ReCycleOnce(changeLog);
                        ballsSum = loser.SexualOrgans.Balls.List.Sum(b => b.BaseValue);
                    }
                }
                // Shrink relevant organs
            }

            if (amountCollected <= amountToLose)
            {
                loser.Essence.Masculinity.InvokeEssenceChange();
                return amountCollected;
            }
            amountCollected -= amountToLose;
            loser.Essence.Masculinity.GainEssence(amountCollected);
            loser.Essence.Masculinity.InvokeEssenceChange();
            return amountToLose;

        }

        public static ChangeLog DrainMasc(this BaseCharacter drainer, BaseCharacter loser, int bonusAmount = 0)
        {
            ChangeLog changeLog = new();
            int drainAmount = drainer.DrainAmount(loser) + bonusAmount;
            drainer.GainMasc(loser.LoseMasc(drainAmount, changeLog));
            foreach (EssencePerk perk in drainer.Essence.EssencePerks)
                perk.PerkDrainEssenceEffect(drainer, loser);
            foreach (EssencePerk perk in loser.Essence.EssencePerks)
                perk.PerkGetDrainedEssenceEffect(loser, drainer);
            return changeLog; // TODO fix
        }

        public static bool CanDrainMasc(this BaseCharacter drainFrom)
        {
            SexualOrgans organs = drainFrom.SexualOrgans;
            int dickSum = organs.Dicks.List.Sum(d => d.BaseValue);
            int ballsSum = organs.Balls.List.Sum(b => b.BaseValue);
            return drainFrom.Essence.Masculinity.Amount > 0 || dickSum > 0 || ballsSum > 0;
        }

        public static ChangeLog GiveMasc(this BaseCharacter giver, BaseCharacter receiver) =>
            GiveMasc(giver, receiver,
                giver.Essence.EssenceOptions.SelfDrain
                    ? giver.Essence.GiveAmount.Value
                    : Mathf.Min(giver.Essence.GiveAmount.Value, giver.Essence.Masculinity.Amount));

        public static ChangeLog GiveMasc(this BaseCharacter giver, BaseCharacter receiver, int giveAmount)
        {
            ChangeLog changeLog = new();
            receiver.GainMasc(giver.LoseMasc(giveAmount, changeLog));
            return changeLog;
        }

        #endregion

        #region Femi
        public static int CalcTotalFemi(this BaseCharacter ofCharacter)
        {
            int tot = ofCharacter.Essence.Femininity.Amount;
            tot += ofCharacter.SexualOrgans.Vaginas.TotalEssenceCost();
            tot += ofCharacter.SexualOrgans.Boobs.TotalEssenceCost();
            return tot;
        }
        public static void GainFemi(this BaseCharacter gainer, int toGain)
        {
            gainer.Essence.Femininity.GainEssence(toGain);
            gainer.GrowOrgans();
        }

        public static int LoseFemi(this BaseCharacter loser, int amountToLose, ChangeLog changeLog)
        {
            int amountCollected = loser.Essence.Femininity.LoseEssence(amountToLose);
            if (amountCollected < amountToLose)
            {
                int boobSum = loser.SexualOrgans.Boobs.List.Sum(d => d.BaseValue);
                int vaginaSum = loser.SexualOrgans.Vaginas.List.Sum(b => b.BaseValue);
                const float boobDiv = 0.7f;
                int breakOut = 0;
                while (amountCollected < amountToLose && (boobSum > 0 || vaginaSum > 0))
                {
                    if (MaxLoop < breakOut) break;
                    breakOut++;
                    if (boobSum > 0 && boobSum * boobDiv > vaginaSum)
                    {
                        amountCollected += loser.SexualOrgans.Boobs.ReCycleOnce(changeLog);
                        boobSum = loser.SexualOrgans.Boobs.List.Sum(d => d.BaseValue);
                    }
                    else if (vaginaSum > 0)
                    {
                        amountCollected += loser.SexualOrgans.Vaginas.ReCycleOnce(changeLog);
                        vaginaSum = loser.SexualOrgans.Vaginas.List.Sum(b => b.BaseValue);
                    }
                    else // Something went wrong
                        break;
                }

                // Shrink relevant organs
            }

            if (amountCollected <= amountToLose)
            {
                loser.Essence.Femininity.InvokeEssenceChange();
                return amountCollected;
            }

            amountCollected -= amountToLose;
            loser.Essence.Femininity.GainEssence(amountCollected);
            loser.Essence.Femininity.InvokeEssenceChange();
            return amountToLose;
        }

        public static ChangeLog DrainFemi(this BaseCharacter drainer, BaseCharacter loser, int bonusAmount = 0)
        {
            ChangeLog changeLog = new();
            int drainAmount = drainer.DrainAmount(loser) + bonusAmount;
            drainer.GainFemi(loser.LoseFemi(drainAmount, changeLog));
            foreach (BasicPerk perk in drainer.LevelSystem.PerksOfType(PerkType.Essence))
                if (perk is EssencePerk essencePerk)
                    essencePerk.PerkDrainEssenceEffect(drainer, loser);
            foreach (BasicPerk perk in loser.LevelSystem.PerksOfType(PerkType.Essence))
                if (perk is EssencePerk essencePerk)
                    essencePerk.PerkGetDrainedEssenceEffect(loser, drainer);
            drainer.Essence.Femininity.InvokeEssenceChange();
            return changeLog; // TODO fix
        }

        public static bool CanDrainFemi(this BaseCharacter drainFrom)
        {
            SexualOrgans organs = drainFrom.SexualOrgans;
            int boobSum = organs.Boobs.List.Sum(d => d.BaseValue);
            int vaginaSum = organs.Vaginas.List.Sum(b => b.BaseValue);
            return drainFrom.Essence.Femininity.Amount > 0 || boobSum > 0 || vaginaSum > 0;
        }

        public static ChangeLog GiveFemi(this BaseCharacter giver, BaseCharacter receiver) =>
            GiveFemi(giver, receiver, giver.Essence.EssenceOptions.SelfDrain
                    ? giver.Essence.GiveAmount.Value
                    : Mathf.Min(giver.Essence.GiveAmount.Value, giver.Essence.Femininity.Amount));

        public static ChangeLog GiveFemi(this BaseCharacter giver, BaseCharacter receiver, int giveAmount)
        {
            ChangeLog changeLog = new();
            receiver.GainFemi(giver.LoseFemi(giveAmount, changeLog));
            return changeLog;
        }

        #endregion
    }

    public class ChangeLog
    {
        public List<string> DrainLogs { get; } = new();
        public List<string> GainLogs { get; } = new();
        public void LogDrainChange(string toLog) => DrainLogs.Add(toLog);
        public void LogGainChange(string toLog) => GainLogs.Add(toLog);
    }
}