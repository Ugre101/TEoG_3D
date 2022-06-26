using System;
using Character.Organs.Fluids;
using Safe_to_Share.Scripts.CustomClasses;
using UnityEngine;

namespace Character.SexStatsStuff
{
    [Serializable]
    public class SexStats : ITickHour
    {
        [SerializeField] int totalOrgasms;
        [SerializeField] int currentArousal;
        [SerializeField] ForeignFluids fluidsOnBody = new();
        [SerializeField] ForeignFluids fluidsInStomach = new();
        [SerializeField] BaseConstIntStat baseMaxArousal = new(100);
        [SerializeField] BaseConstIntStat maxCasterOrgasms = new(2);
        [SerializeField] BaseConstFloatStat gainArousalFactor = new(1f);
        [SerializeField] BaseConstFloatStat giveArousalFactor = new(1f);

        public bool CanOrgasmMore => SessionOrgasms < MaxCasterOrgasms.Value;

        public int Arousal
        {
            get => currentArousal;
            private set
            {
                currentArousal = value;
                ArousalChange?.Invoke(currentArousal);
                ArousalSliderChange?.Invoke((float)currentArousal / MaxArousal);
            }
        }

        public int TotalOrgasms
        {
            get => totalOrgasms;
            private set
            {
                totalOrgasms = value;
                OrgasmChange?.Invoke(totalOrgasms);
            }
        }

        int SessionOrgasms { get; set; }
        int UsedSessionOrgasms { get; set; }
        public int MaxArousal => BaseMaxArousal.Value + SessionOrgasmMultiplier();

        public BaseConstIntStat MaxCasterOrgasms => maxCasterOrgasms;

        public BaseConstIntStat BaseMaxArousal => baseMaxArousal;

        public ForeignFluids FluidsOnBody => fluidsOnBody;

        public BaseConstFloatStat GainArousalFactor => gainArousalFactor;

        public BaseConstFloatStat GiveArousalFactor => giveArousalFactor;

        public ForeignFluids FluidsInStomach => fluidsInStomach;

        public bool TickHour(int ticks) => MaxCasterOrgasms.Mods.TickHour(ticks) | BaseMaxArousal.Mods.TickHour(ticks);
        public event Action<float> ArousalSliderChange;
        public event Action<int> OrgasmChange, ArousalChange;


        public bool HasEnoughOrgasms(int needed) => needed <= SessionOrgasms - UsedSessionOrgasms;

        int SessionOrgasmMultiplier() => SessionOrgasms > 0 ? Mathf.FloorToInt(Mathf.Pow(2.05f, SessionOrgasms)) : 0;

        public void NewSession()
        {
            SessionOrgasms = 0;
            UsedSessionOrgasms = 0;
        }

        public bool UseSessionOrgasms(int uses)
        {
            if (uses > SessionOrgasms - UsedSessionOrgasms)
                return false;
            UsedSessionOrgasms += uses;
            return true;
        }

        public int GainArousal(float gain)
        {
            int timesOrgasmed = 0;
            Arousal += Mathf.RoundToInt(gain * GainArousalFactor.Value);
            while (MaxArousal <= Arousal)
            {
                Arousal -= MaxArousal;
                TotalOrgasms++;
                SessionOrgasms++;
                timesOrgasmed++;
            }

            return timesOrgasmed;
        }
    }
}