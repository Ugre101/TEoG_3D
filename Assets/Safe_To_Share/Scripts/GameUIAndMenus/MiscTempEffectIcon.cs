using System.Collections.Generic;
using System.Linq;
using Character.StatsStuff.Mods;
using Safe_To_Share.Scripts.Static;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.GameUIAndMenus {
    public sealed class MiscTempEffectIcon : MonoBehaviour {
        [SerializeField] Image icon;
        [SerializeField] TextMeshProUGUI timeLeft;

        List<TempIntMod> mod;

        void OnDestroy() => DateSystem.NewHour -= UpdateTimeLeft;

        public void Setup(List<TempIntMod> tempIntMod) {
            mod = tempIntMod;
            DateSystem.NewHour += UpdateTimeLeft;
            UpdateTimeLeft(101);
        }

        void UpdateTimeLeft(int obj) {
            var max = mod.Max(m => m.HoursLeft);
            if (max <= 0)
                Destroy(gameObject);
            timeLeft.text = $"{max}h";
        }
    }
}