using System.Linq;
using Character;
using Character.EssenceStuff;
using Character.Organs;
using Character.Organs.OrgansContainers;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts.Character.EssenceStuff.Perks {
    [CreateAssetMenu(menuName = "Character/Essence/Create GenderMoprh Perk", fileName = "Gender morph perk", order = 0)]
    public sealed class GenderMorph : EssencePerk {
        public enum MorphToGender {
            Disabled,
            Male,
            Female,
            CuntBoy,
            DickGirl,
            Futanari,
        }

        const int AmountToLose = 100;
        [SerializeField, Range(25, 200),] int bonus = 30;
        [SerializeField, Range(0, 25),] int stableGain = 15;

        readonly Random rng = new();

        public override void OnCasterOrgasmPerkEffect(BaseCharacter perkOwner, BaseCharacter partner) {
            var morphTo = perkOwner.Essence.EssenceOptions.MorphPartnerToGender;
            var organs = partner.SexualOrgans;
            ChangeLog changeLog = new();
            switch (morphTo) {
                case MorphToGender.Disabled:
                    return;
                case MorphToGender.Male:
                    partner.GainMasc(partner.LoseFemi(AmountToLose, changeLog) + bonus);
                    break;
                case MorphToGender.Female:
                    partner.GainFemi(partner.LoseMasc(AmountToLose, changeLog) + bonus);
                    break;
                case MorphToGender.CuntBoy:
                    MorphToCuntBoy(partner, organs, changeLog);
                    break;
                case MorphToGender.DickGirl:
                    MorphToDickgirl(partner, changeLog);
                    break;
                case MorphToGender.Futanari:
                    var manSum = organs.Balls.BaseList.Sum(b => b.BaseValue) +
                                 organs.Dicks.BaseList.Sum(d => d.BaseValue);
                    var femaleSum = organs.Vaginas.BaseList.Sum(v => v.BaseValue) +
                                    organs.Boobs.BaseList.Sum(b => b.BaseValue);
                    if (manSum < femaleSum)
                        partner.GainMasc(partner.LoseFemi(AmountToLose, changeLog) + bonus);
                    else
                        partner.GainFemi(partner.LoseMasc(AmountToLose, changeLog) + bonus);
                    break;
            }

            partner.InvokeUpdateAvatar();
        }

        void MorphToDickgirl(BaseCharacter partner, ChangeLog changeLog) {
            partner.Essence.StableEssence.BaseValue += stableGain;
            if (partner.SexualOrgans.Vaginas.HaveAny()) {
                var gain = partner.SexualOrgans.Vaginas.ReCycleOnce(changeLog);
                partner.Essence.Femininity.GainEssence(gain / 2);
                partner.Essence.Masculinity.GainEssence(gain / 2);
            }

            if (partner.SexualOrgans.Boobs.HaveAny())
                partner.SexualOrgans.Boobs.TryGrowSmallest(partner.Essence.Femininity);
            else
                partner.SexualOrgans.Boobs.TryGrowNew(partner.Essence.Femininity);
            partner.GrowOrgans();
        }

        void MorphToCuntBoy(BaseCharacter partner, SexualOrgans organs, ChangeLog changeLog) {
            partner.Essence.StableEssence.BaseValue += stableGain;
            var toTakeFrom = new BaseOrgansContainer[] { organs.Balls, organs.Dicks, organs.Boobs, };
            var withOrgans = toTakeFrom.Where(c => c.HaveAny()).ToArray();
            var toGain = withOrgans[rng.Next(withOrgans.Length)].ReCycleOnce(changeLog);
            partner.Essence.Femininity.GainEssence(toGain);
            if (partner.SexualOrgans.Vaginas.HaveAny())
                partner.SexualOrgans.Vaginas.TryGrowSmallest(partner.Essence.Femininity);
            else
                partner.SexualOrgans.Vaginas.TryGrowNew(partner.Essence.Femininity);
            partner.GrowOrgans();
        }
    }
}