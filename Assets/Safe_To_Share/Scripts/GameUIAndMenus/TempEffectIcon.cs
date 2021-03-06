using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Character.StatsStuff.Mods;
using GameUIAndMenus.EffectUI;
using Items;
using Safe_To_Share.Scripts.Static;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

namespace GameUIAndMenus
{
    public class TempEffectIcon : EffectIcon
    {
        [SerializeField] TextMeshProUGUI timeLeft;
        public IObjectPool<TempEffectIcon> pool;
        Item item;
        List<TempIntMod> mod;

        protected override string HoverText
        {
            get
            {
                StringBuilder sb = new();
                sb.AppendLine(item.Title);
                sb.Append(item.TempEffectDesc);
                return sb.ToString();
            }
        }

        void OnDisable()
        {
            pool.Release(this);
        }

        public void Setup(Item gotItem, List<TempIntMod> tempIntMod)
        {
            icon.sprite = gotItem.Icon;
            item = gotItem;
            mod = tempIntMod;
            DateSystem.NewHour += UpdateTimeLeft;
            UpdateTimeLeft(101);
        }

        public void Clear()
        {
            icon.sprite = null;
            item = null;
            mod = null;
            DateSystem.NewHour -= UpdateTimeLeft;
        }

        void OnDestroy() => DateSystem.NewHour -= UpdateTimeLeft;

        void UpdateTimeLeft(int obj)
        {
            int max = mod.Max(m => m.HoursLeft);
            timeLeft.text = $"{max}h";
            if (max <= 0)
                Destroy(gameObject);
        }
    }
}