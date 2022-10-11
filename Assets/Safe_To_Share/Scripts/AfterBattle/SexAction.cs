using System;
using System.Collections.Generic;
using System.Linq;
using Character;
using Character.BodyStuff;
using Character.EssenceStuff;
using Character.Organs;
using Character.Organs.Fluids;
using Character.Organs.OrgansContainers;
using Character.PregnancyStuff;
using Character.SexStatsStuff;
using Safe_To_Share.Scripts.AfterBattle.Vore;
using Safe_to_Share.Scripts.CustomClasses;
using UnityEngine;
using Random = System.Random;

namespace Safe_To_Share.Scripts.AfterBattle
{
    [CreateAssetMenu(fileName = "Sex Act", menuName = "AfterBattle/Sex Act")]
    public class SexAction : AfterBattleBaseAction
    {
        [SerializeField] ArousalGain arousalGain;
        [SerializeField] List<NeededOrgans> casterNeedsSexualOrgan = new();
        [SerializeField] List<NeededOrgans> partnerNeedsSexualOrgan = new();
        [SerializeField] List<NeededBody> casterNeedsBody = new();
        [SerializeField] List<NeededBody> partnerNeedsBody = new();
        [SerializeField] HeightCompression heightCompression;

        [Header("Pregnancy"), SerializeField,] bool canImpregnate;

        [SerializeField] bool canGetImpregnated;
        [SerializeField] SexualOrganType organToBeImpregnated;

        [Header("Fluids"), SerializeField,] List<CumFluidsFromInto> cumFluidsFromInto = new();

        readonly Random rng = new();

        string ImpregnateOrganTitle => organToBeImpregnated.ToString().ToLower();

        public override bool CanUse(BaseCharacter giver, BaseCharacter receiver) =>
            giver.SexStats.CanOrgasmMore &&
            MeetsBodyReq(giver, casterNeedsBody) &&
            MeetsOrganReq(giver, casterNeedsSexualOrgan) &&
            MeetsBodyReq(receiver, partnerNeedsBody) &&
            MeetsOrganReq(receiver, partnerNeedsSexualOrgan) &&
            heightCompression.PassHeightReq(giver, receiver);

        static bool MeetsOrganReq(BaseCharacter character, IReadOnlyCollection<NeededOrgans> needOrgans)
            => needOrgans.Count <= 0 || needOrgans.Any(c => c.MeetReq(character));

        static bool MeetsBodyReq(BaseCharacter character, IReadOnlyCollection<NeededBody> bodyNeeds)
            => bodyNeeds.Count <= 0 || bodyNeeds.Any(c => c.MeetReq(character));

        public override SexActData Use(AfterBattleActor caster, AfterBattleActor target)
        {
            data.AfterText.Clear();
            ArousalGain.GainInfo gain = arousalGain.Gain(caster.Actor, target.Actor);
            for (int i = 0; i < gain.TimesCasterOrgasmed; i++)
                OnCasterOrgasm(caster, target);
            for (int i = 0; i < gain.TimesPartnerOrgasmed; i++)
                OnTargetOrgasm(caster, target);
            return data;
        }

        void OnCasterOrgasm(AfterBattleActor caster, AfterBattleActor target)
        {
            if (canImpregnate && CanGetImpregnated(caster.Actor, target.Actor))
                data.AfterText.Add($"You impregnated {target.Actor.Identity.FirstName}'s {ImpregnateOrganTitle}");
            foreach (EssencePerk perk in caster.Actor.Essence.EssencePerks)
                perk.OnCasterOrgasmPerkEffect(caster.Actor, target.Actor);
            HandleFluids(caster, target);
        }

        void HandleFluids(AfterBattleActor caster, AfterBattleActor target)
        {
            foreach (var fluidsInto in cumFluidsFromInto)
                if (fluidsInto.FromPartner)
                    Test(target.Actor, caster.Actor, fluidsInto);
                else
                    Test(caster.Actor, target.Actor, fluidsInto);
        }

        static void Test(BaseCharacter from, BaseCharacter into, CumFluidsFromInto fluidsInto)
        {
            var foreignFluidsList = new List<ForeignFluids>();
            switch (fluidsInto.Into)
            {
                case CumFluidsFromInto.IntoForeignFluids.Vagina:
                    foreignFluidsList.AddRange(into.SexualOrgans.Vaginas.List.Select(v => v.Womb.ForeignFluids));
                    break;
                case CumFluidsFromInto.IntoForeignFluids.Stomach:
                    foreignFluidsList.Add(into.SexStats.FluidsInStomach);
                    break;
                case CumFluidsFromInto.IntoForeignFluids.Body:
                    foreignFluidsList.Add(into.SexStats.FluidsOnBody);
                    break;
                case CumFluidsFromInto.IntoForeignFluids.Boobs:
                    foreignFluidsList.AddRange(into.SexualOrgans.Boobs.List.Select(v => v.Womb.ForeignFluids));
                    break;
                case CumFluidsFromInto.IntoForeignFluids.Balls:
                    foreignFluidsList.AddRange(into.SexualOrgans.Balls.List.Select(v => v.Womb.ForeignFluids));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            SexExtensions.Orgasm(from, fluidsInto.Form, foreignFluidsList);
        }

        void OnTargetOrgasm(AfterBattleActor caster, AfterBattleActor target)
        {
            if (canGetImpregnated && CanGetImpregnated(target.Actor, caster.Actor))
                data.AfterText.Add($"Your {ImpregnateOrganTitle} got impregnated by {target.Actor.Identity.FirstName}");
            foreach (EssencePerk essencePerk in caster.Actor.Essence.EssencePerks)
                essencePerk.OnPartnerOrgasmPerkEffect(caster.Actor, target.Actor);
        }

        bool CanGetImpregnated(BaseCharacter father, BaseCharacter mother)
        {
            if (!mother.SexualOrgans.Containers.TryGetValue(organToBeImpregnated, out OrgansContainer motherCon) ||
                !motherCon.HaveAny())
                return false;
            if (motherCon.List.Any(c => c.Womb.HasFetus))
                return false;
            return father.TryImpregnate(mother, motherCon.GetRandomOrgan());
        }

        [Serializable]
        struct NeededBody
        {
            [SerializeField] BodyStatType bodyType;
            [SerializeField] NeedToMet minSize, maxSize;

            public bool MeetReq(BaseCharacter character)
            {
                float value = character.Body.BodyStats[bodyType].Value;
                return (!minSize.Need || !(value < minSize.Size)) &&
                       (!maxSize.Need || !(maxSize.Size < value));
            }
        }

        [Serializable]
        struct HeightCompression
        {
            [SerializeField] bool active;
            [SerializeField] float playerIsXTimesTaller;

            public bool PassHeightReq(BaseCharacter caster, BaseCharacter partner)
            {
                if (!active)
                    return true;
                return caster.Body.Height.Value >= partner.Body.Height.Value * playerIsXTimesTaller;
            }
        }

        [Serializable]
        struct NeededOrgans
        {
            [SerializeField] SexualOrganType organType;
            [SerializeField] NeedToMet minSize, maxSize, amount;

            public SexualOrganType OrganType => organType;

            public bool MeetReq(BaseCharacter character)
            {
                OrgansContainer organsContainer = character.SexualOrgans.Containers[OrganType];
                return (!minSize.Need || !(organsContainer.Biggest < minSize.Size)) &&
                       (!maxSize.Need || !(maxSize.Size < organsContainer.List.Min(o => o.Value))) &&
                       (!amount.Need || !(organsContainer.List.Count() < amount.Size));
            }
        }

        [Serializable]
        class NeedToMet
        {
            [SerializeField] bool need;
            [SerializeField] float size;
            public bool Need => need;
            public float Size => size;
        }

        [Serializable]
        class ArousalGain
        {
            [SerializeField] int casterBaseAmount;
            [SerializeField] int targetBaseAmount;

            [SerializeField] RngValue rng;
            //     [SerializeField]  List<AffectedBySexualOrgan> giverAffectedBySexualOrganFromReceiver;
            //     [SerializeField]  List<AffectedBySexualOrgan> receiverAffectedBySexualOrganFromGiver;

            public GainInfo Gain(BaseCharacter caster, BaseCharacter receiver)
            {
                int casterArousal = GainArousal(caster, receiver, casterBaseAmount, out int casterOrgasmed);
                int partnerArousal = GainArousal(receiver, caster, targetBaseAmount, out int partnerOrgasmed);
                string returnText =
                    $"{caster.Identity.FirstName} gained {casterArousal} arousal and {receiver.Identity.FirstName} gained {partnerArousal} arousal.";
                returnText = ReturnTextOrgasms(caster, casterOrgasmed, returnText);
                returnText = ReturnTextOrgasms(receiver, partnerOrgasmed, returnText);
                return new GainInfo(returnText, casterOrgasmed, partnerOrgasmed);

                int GainArousal(BaseCharacter gainer, BaseCharacter giver, float gain, out int orgasms)
                {
                    float gGain = gain * rng.GetRandomValue;
                    int i = Mathf.RoundToInt(gGain * giver.SexStats.GiveArousalFactor.Value);
                    orgasms = gainer.SexStats.GainArousal(i);
                    return i;
                }

                static string ReturnTextOrgasms(BaseCharacter giver, int gOrgasm, string returnText)
                {
                    if (gOrgasm <= 0)
                        return returnText;
                    returnText += $"\n{giver.Identity.FirstName} orgasmed";
                    if (gOrgasm > 1)
                        returnText += $" {gOrgasm} times!";
                    return returnText;
                }
            }

            public struct GainInfo
            {
                public string Summary { get; }
                public int TimesCasterOrgasmed { get; }
                public int TimesPartnerOrgasmed { get; }

                public GainInfo(string summary, int timesCasterOrgasmed, int timesPartnerOrgasmed)
                {
                    Summary = summary;
                    TimesCasterOrgasmed = timesCasterOrgasmed;
                    TimesPartnerOrgasmed = timesPartnerOrgasmed;
                }
            }
        }

        [Serializable]
        struct CumFluidsFromInto
        {
            public enum IntoForeignFluids
            {
                Vagina,
                Stomach,
                Body,
                Boobs,
                Balls,
            }

            [SerializeField] bool fromPartner;
            [SerializeField] SexualOrganType form;
            [SerializeField] IntoForeignFluids into;

            public bool FromPartner => fromPartner;

            public SexualOrganType Form => form;

            public IntoForeignFluids Into => into;
        }
    }

    [Serializable]
    public struct SexActData
    {
        [SerializeField] AddedAnimations.SexAnimations giver, receiver;

        [field: SerializeField] public SexActionAnimation SexActionAnimation { get; private set; }
        [SerializeField] string text;
        [SerializeField] List<string> afterText;
        public AddedAnimations.SexAnimations Giver => giver;

        public AddedAnimations.SexAnimations Receiver => receiver;

        public List<string> AfterText => afterText;

        public string TitleText => text;
    }
}