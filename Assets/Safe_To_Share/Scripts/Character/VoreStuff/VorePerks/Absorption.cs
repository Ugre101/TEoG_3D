using System.Collections.Generic;
using Character.BodyStuff;
using Safe_To_Share.Scripts.Character.VoreStuff.VoreDigestionModes;
using Safe_to_Share.Scripts.CustomClasses;
using UnityEngine;

namespace Character.VoreStuff.VorePerks {
    [CreateAssetMenu(fileName = "Absorb perk", menuName = "Character/Vore/Vore Absorb perk", order = 0)]
    public sealed class Absorption : VorePerkNewDigestionMode {
        const float percentAbsorbed = 13f;
        static float? inPercent;
        public override string DigestionMode => VoreOrganDigestionMode.Absorption;
        public override IEnumerable<VoreType> OrganType => new[] { VoreType.Oral, };

        static float PercentAbsorbed {
            get {
                if (inPercent.HasValue)
                    return inPercent.Value;
                inPercent = percentAbsorbed / 100f;
                return inPercent.Value;
            }
        }

        public override void OnDigestionTick(BaseCharacter pred, VoreOrgan voreOrgan, VoreType type) {
            for (var index = voreOrgan.PreysIds.Count; index-- > 0;)
                AbsorbPrey(pred, voreOrgan, index, type);
        }

        static void AbsorbPrey(BaseCharacter pred, VoreOrgan voreOrgan, int index, VoreType type) {
            var preyId = voreOrgan.PreysIds[index];
            if (!VoredCharacters.PreyDict.TryGetValue(preyId, out var prey))
                return;
            float toDigest = pred.Vore.digestionStrength.Value;
            if (AbsorbBodyStat(pred, prey, BodyStatType.Height, toDigest))
                FullyAbsorbed(pred, voreOrgan, prey, preyId);
            else
                prey.SetAltProgress(Mathf.Clamp((prey.StartWeight - prey.Body.Weight) / prey.StartWeight * 100f, 0f,
                    100f));
        }

        static void FullyAbsorbed(BaseCharacter pred, VoreOrgan voreOrgan, Prey prey, int preyId) {
            pred.OnStomachDigestion(prey, VoreOrganDigestionMode.Absorption);
            // EventLog.AddEvent($"{pred.Identity.FirstName} has fully absorbed {prey.Identity.FullName}");
            voreOrgan.RemovePrey(preyId);
            VoredCharacters.RemovePrey(prey);
        }

        static bool AbsorbBodyStat(BaseCharacter pred, BaseCharacter prey, BodyStatType type, float toDigest) {
            if (!pred.Body.BodyStats.TryGetValue(type, out var predBodyStat) ||
                !prey.Body.BodyStats.TryGetValue(type, out var preyBodyStat))
                return false;
            var digested = GetValue(toDigest, preyBodyStat);
            var bodyStatDiff = preyBodyStat.BaseValue / predBodyStat.BaseValue;
            predBodyStat.BaseValue += digested * PercentAbsorbed * bodyStatDiff;
            var lacking = toDigest - digested;
            if (lacking <= 0)
                return false;
            var left = GetTheRest(prey.Body, lacking);
            predBodyStat.BaseValue += left * PercentAbsorbed * bodyStatDiff / 2f; //only half the gain 
            return left < lacking;
        }

        static float GetTheRest(Body body, float lacking) {
            float digested = 0;
            foreach (var bodyStat in body.BodyStats.Values) {
                if (lacking <= digested)
                    return digested;
                digested += GetValue(lacking - digested, bodyStat);
            }

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
    }
}