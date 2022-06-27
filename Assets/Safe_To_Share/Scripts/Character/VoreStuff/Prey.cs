using System;
using System.Linq;
using Character.BodyStuff;
using Character.PregnancyStuff;
using Character.StatsStuff;
using Safe_to_Share.Scripts.CustomClasses;
using Safe_To_Share.Scripts.Static;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Character.VoreStuff
{
    [Serializable]
    public class Prey : BaseCharacter
    {
        [SerializeField] float startWeight;
        [SerializeField] DateSave voredDate;
        [SerializeField] float altProgress;
        [SerializeField] string specialDigestion;
        public bool havePleaded;
        public Prey(BaseCharacter character) : base(character)
        {
            character.Unsub();
            startWeight = character.Body.Weight;
            voredDate = DateSystem.Save();
        }

        public float StartWeight => startWeight;

        public DateSave VoredDate => voredDate;

        public float AltProgress => altProgress;

        public string SpecialDigestion => specialDigestion;

        public float DigestionProgress => 100f - Mathf.RoundToInt(Body.Weight / StartWeight * 100f);
        

        public void SetSpecialDigestionMode(string mode) => specialDigestion = mode;

        public float Digest(float toDigest, bool predIsPlayer)
        {
            float digested = GetValue(toDigest, Body.Fat);
            if (digested < toDigest)
                digested += GetValue(toDigest - digested, Body.Muscle);
            if (digested < toDigest)
                digested += GetValue(toDigest - digested, Body.Height);
      
            return digested;
        }


        static float GetValue(float toDigest, BaseFloatStat bodyStat)
        {
            if (bodyStat.BaseValue <= 0)
                return 0;
            if (bodyStat.BaseValue > toDigest)
            {
                bodyStat.BaseValue -= toDigest;
                return toDigest;
            }
            return TakeTheLast(bodyStat);
        }

        private static float TakeTheLast(BaseFloatStat bodyStat)
        {
            float digested = bodyStat.BaseValue;
            bodyStat.BaseValue = 0;
            return digested;
        }

        public float AltDigest(float percentProgress = 0.1f)
        {
            altProgress = AltProgress + percentProgress;
            return AltProgress;
        }

        int ticksImpreg = 0;
        public bool EndosomaTryImpregnate(BaseCharacter pred)
        {
            ticksImpreg++;
            if (ticksImpreg > 33)
            {
                ticksImpreg = 0;
                if (!SexualOrgans.Vaginas.HaveAny() || SexualOrgans.Vaginas.List.All(v => v.Womb.HasFetus)) return false;
                var emptyWomb = SexualOrgans.Vaginas.List.FirstOrDefault(v => v.Womb.HasFetus == false);
                return emptyWomb != null && pred.TryImpregnate(this, emptyWomb);
            }
            return false;
        }

        public override void TickHour(int ticks = 1)
        {
            Stats.TickHour(ticks);
            Stats.TickMin(ticks * 60);
            PregnancySystem.TickHour(ticks);
            Vore.TickHour(this,ticks);
            Essence.TickHour(ticks);
            SexualOrgans.TickHour(ticks);
            SexualOrgans.TickMin(ticks * 60);
            Body.TickHour(ticks);
            SexStats.TickHour(ticks);
            Body.BurnFatHour(ticks);
        }
    }
}