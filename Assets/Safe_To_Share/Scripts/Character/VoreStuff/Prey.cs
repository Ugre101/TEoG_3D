using System;
using System.Linq;
using Character.BodyStuff;
using Character.CreateCharacterStuff;
using Character.PregnancyStuff;
using Safe_to_Share.Scripts.CustomClasses;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Character.VoreStuff {
    [Serializable]
    public class Prey : BaseCharacter {
        [SerializeField] float startWeight;
        [SerializeField] DateSave voredDate;
        [SerializeField] float altProgress;
        [SerializeField] string specialDigestion;
        public bool havePleaded;

        int ticksImpreg;

        public Prey(BaseCharacter character) : base(character) {
            character.Unsub();
            startWeight = character.Body.Weight;
            voredDate = DateSystem.Save();
        }

        public Prey(CreateCharacter character) : base(character) {
            startWeight = Body.Weight;
            voredDate = DateSystem.Save();
        }

        public float StartWeight => startWeight;

        public DateSave VoredDate => voredDate;

        public float AltProgress => altProgress;

        public string SpecialDigestion => specialDigestion;

        public float DigestionProgress => 100f - Mathf.RoundToInt(Body.Weight / StartWeight * 100f);


        public void SetSpecialDigestionMode(string mode) => specialDigestion = mode;

        public float Digest(float toDigest, bool predIsPlayer) {
            var digested = GetValue(toDigest, Body.Fat);
            if (digested < toDigest)
                digested += GetValue(toDigest - digested, Body.Muscle);
            if (digested < toDigest)
                digested += GetValue(toDigest - digested, Body.Height);

            return digested;
        }


        static float GetValue(float toDigest, BaseFloatStat bodyStat) {
            if (bodyStat.BaseValue <= 0)
                return 0;
            if (bodyStat.BaseValue < toDigest)
                return TakeTheLast(bodyStat);
            bodyStat.BaseValue -= toDigest;
            return toDigest;
        }

        static float TakeTheLast(BaseFloatStat bodyStat) {
            var digested = bodyStat.BaseValue;
            bodyStat.BaseValue = 0;
            return digested;
        }

        public float AltDigest(float percentProgress = 0.1f) {
            altProgress = AltProgress + percentProgress;
            return AltProgress;
        }

        public void SetAltProgress(float progress) {
            altProgress = progress;
        }

        public bool EndosomaTryImpregnate(BaseCharacter pred) {
            ticksImpreg++;
            if (ticksImpreg <= 33)
                return false;
            ticksImpreg = 0;
            if (!SexualOrgans.Vaginas.HaveAny() || SexualOrgans.Vaginas.BaseList.All(v => v.Womb.HasFetus))
                return false;
            var emptyWomb = SexualOrgans.Vaginas.BaseList.FirstOrDefault(v => v.Womb.HasFetus == false);
            return emptyWomb != null && pred.TryImpregnate(this, emptyWomb);
        }

        public override void TickHour(int ticks = 1) {
            Stats.TickHour(ticks);
            Stats.TickMin(ticks * 60);
            PregnancySystem.TickHour(ticks);
            Vore.TickHour(this, ticks);
            Essence.TickHour(ticks);
            SexualOrgans.TickHour(ticks);
            SexualOrgans.TickMin(ticks * 60);
            Body.TickHour(ticks);
            SexStats.TickHour(ticks);
            Body.BurnFatHour(ticks);
        }
    }
}