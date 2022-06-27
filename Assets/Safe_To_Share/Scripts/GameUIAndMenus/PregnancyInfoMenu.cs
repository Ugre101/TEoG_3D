using System.Collections.Generic;
using System.Linq;
using System.Text;
using Character.Organs;
using Character.PregnancyStuff;
using CustomClasses;
using Safe_to_Share.Scripts.CustomClasses;
using Safe_To_Share.Scripts.Static;
using TMPro;
using UnityEngine;

namespace GameUIAndMenus
{
    public class PregnancyInfoMenu : GameMenu
    {
        [SerializeField] TextMeshProUGUI fert, baseFert, veri, baseViri;
        [SerializeField] TextMeshProUGUI pregSpeed;
        [SerializeField] TextMeshProUGUI pregInfo;

        void OnEnable()
        {
            ShowFert();
            ShowViri();
            ShowPregnancySpeed();
            PrintPregInfo();
        }

        void ShowViri()
        {
            BaseIntStat virility = Player.PregnancySystem.Virility;
            veri.text = $"Virility\n{virility.Value}";
            baseViri.text = virility.Value - virility.BaseValue > 1 ? $"\n{virility.BaseValue} base" : string.Empty;
        }

        void ShowFert()
        {
            BaseIntStat fertility = Player.PregnancySystem.Fertility;
            fert.text = $"Fertility\n{fertility.Value}";
            baseFert.text = fertility.Value - fertility.BaseValue > 1 ? $"\n{fertility.BaseValue} base" : string.Empty;
        }

        void ShowPregnancySpeed()
        {
            BaseConstIntStat preg = Player.PregnancySystem.PregnancySpeed;
            pregSpeed.text = $"Pregnancy Speed\n{preg.Value}";
        }

        void PrintPregInfo()
        {
            StringBuilder sb = new();
            if (Player.SexualOrgans.Vaginas.List.Any(v => v.Womb.HasFetus))
                for (int index = 0; index < Player.SexualOrgans.Vaginas.List.Count(); index++)
                    PrintWomb(sb, index);
            sb.AppendLine();
            int timesPregnant = Player.PregnancySystem.TimesPregnant;
            sb.Append("You have");
            sb.AppendLine(timesPregnant > 0 ? $" been pregnant {timesPregnant} times" : " never been pregnant.");
            sb.AppendLine();
            sb.Append("You have ");
            int impregnated = Player.PregnancySystem.TimesImpregnated;
            sb.AppendLine(impregnated > 0 ? $" impregnated {impregnated} times." : "never impregnated anyone.");
            pregInfo.text = sb.ToString();
        }

        void PrintWomb(StringBuilder sb, int index)
        {
            BaseOrgan baseOrgan = Player.SexualOrgans.Vaginas.List.ElementAt(index);
            List<Fetus> wombList = baseOrgan.Womb.FetusList;
            if (!wombList.Any())
                return;
            sb.AppendLine($"Your {(index + 1).IntToFirstSecondEtc()} vagina is pregnant with ");
            switch (wombList.Count)
            {
                case 1:
                    sb.Append($" a {wombList[0].DaysOld} days old child.");
                    break;
                case 2:
                    sb.Append($" a pair of {wombList[0].DaysOld} days old twins.");
                    break;
                default:
                    sb.Append($" {wombList.Count} {wombList[0].DaysOld} days old growing children.");
                    break;
            }

            sb.AppendLine();
        }
    }
}