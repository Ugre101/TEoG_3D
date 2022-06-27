using System.Collections.Generic;
using System.Linq;
using System.Text;
using Character;
using Character.StatsStuff.Mods;
using GameUIAndMenus.EffectUI;
using Safe_To_Share.Scripts.Static;
using TMPro;
using UnityEngine;

namespace GameUIAndMenus
{
    public class SleepTempEffectIcon : EffectIcon
    {
        [SerializeField] TextMeshProUGUI timeLeft;

        List<TempIntMod> mods;
        string sleepDesc;
        float sleepTier;

        protected override string HoverText => sleepDesc;

        void OnDisable() => DateSystem.NewHour -= UpdateTimeLeft;

        public void Setup(List<TempIntMod> tempIntMod, float tier)
        {
            mods = tempIntMod;
            sleepTier = tier;
            DateSystem.NewHour += UpdateTimeLeft;
            UpdateTimeLeft(101);
            StringBuilder sb = new();
            sb.AppendLine(SleepExtensions.SleepTitle(sleepTier));
            sleepDesc = sb.ToString();
        }

        void UpdateTimeLeft(int obj)
        {
            int max = mods.Max(m => m.HoursLeft);
            timeLeft.text = $"{max}h";
            if (max <= 0)
                gameObject.SetActive(false);
        }
    }
}