using System.Collections.Generic;
using Character.BodyStuff;
using Character.VoreStuff.VoreDigestionModes;
using Safe_to_Share.Scripts.CustomClasses;
using UnityEngine;

namespace Character.VoreStuff.VorePerks
{
    [CreateAssetMenu(fileName = "Absorb perk", menuName = "Character/Vore/Vore Absorb perk", order = 0)]
    public class Absorption : VorePerkNewDigestionMode
    {
        const float percentAbsorbed = 13f;
        static float? inPercent;
        public override string DigestionMode => VoreOrganDigestionMode.Absorption;
        public override IEnumerable<VoreType> OrganType => new[] { VoreType.Oral, };

        static float PercentAbsorbed
        {
            get
            {
                if (inPercent.HasValue)
                    return inPercent.Value;
                inPercent = percentAbsorbed / 100f;
                return inPercent.Value;
            }
        }

        public override void OnDigestionTick(BaseCharacter pred, VoreOrgan voreOrgan, VoreType type)
        {
            for (int index = voreOrgan.PreysIds.Count; index-- > 0;)
                AbsorbPrey(pred, voreOrgan, index, type);
        }

        static void AbsorbPrey(BaseCharacter pred, VoreOrgan voreOrgan, int index, VoreType type)
        {
            int preyId = voreOrgan.PreysIds[index];
            if (!VoredCharacters.PreyDict.TryGetValue(preyId, out Prey prey))
                return;
            float toDigest = pred.Vore.digestionStrength.Value;
            if (AbsorbBodyStat(pred, prey, BodyStatType.Height, toDigest))
                FullyAbsorbed(pred, voreOrgan, prey, preyId);
        }

        static void FullyAbsorbed(BaseCharacter pred, VoreOrgan voreOrgan, Prey prey, int preyId)
        {
            pred.OnStomachDigestion(prey, VoreOrganDigestionMode.Absorption);
            // EventLog.AddEvent($"{pred.Identity.FirstName} has fully absorbed {prey.Identity.FullName}");
            voreOrgan.RemovePrey(preyId);
            VoredCharacters.RemovePrey(prey);
        }

        static bool AbsorbBodyStat(BaseCharacter pred, BaseCharacter prey, BodyStatType type, float toDigest)
        {
            if (!pred.Body.BodyStats.TryGetValue(type, out var predBodyStat) ||
                !prey.Body.BodyStats.TryGetValue(type, out var preyBodyStat))
                return false;
            float digested = GetValue(toDigest, preyBodyStat);
            float bodyStatDiff = preyBodyStat.BaseValue / predBodyStat.BaseValue;
            predBodyStat.BaseValue += digested * PercentAbsorbed * bodyStatDiff;
            float lacking = toDigest - digested;
            if (lacking <= 0)
                return false;
            float left = GetTheRest(prey.Body, lacking);
            predBodyStat.BaseValue += left * PercentAbsorbed * bodyStatDiff / 2f; //only half the gain 
            return left < lacking;
        }

        static float GetTheRest(Body body, float lacking)
        {
            float digested = 0;
            foreach (var bodyStat in body.BodyStats.Values)
            {
                if (lacking <= digested)
                    return digested;
                digested += GetValue(lacking - digested, bodyStat);
            }

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

        static float TakeTheLast(BaseFloatStat bodyStat)
        {
            float digested = bodyStat.BaseValue;
            bodyStat.BaseValue = 0;
            return digested;
        }
    }
}