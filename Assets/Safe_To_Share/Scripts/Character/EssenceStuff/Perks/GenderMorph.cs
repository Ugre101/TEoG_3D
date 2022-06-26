using Character;
using Character.EssenceStuff;
using Character.Organs;
using Character.Organs.OrgansContainers;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Character.EssenceStuff.Perks
{

    [CreateAssetMenu(menuName = "Character/Essence/Create GenderMoprh Perk", fileName = "Gender morph perk", order = 0)]
    public class GenderMorph : EssencePerk
    {
        private const int AmountToLose = 100;
        [SerializeField, Range(25, 200)] int bonus = 30;
        [SerializeField, Range(0, 25)] int stableGain = 15;

        readonly System.Random rng = new();
        public override void OnCasterOrgasmPerkEffect(BaseCharacter perkOwner, BaseCharacter partner)
        {
            MorphToGender morphTo = perkOwner.Essence.EssenceOptions.MorphPartnerToGender;
            var organs = partner.SexualOrgans;
            ChangeLog changeLog = new();
            switch (morphTo)
            {
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
                    int manSum = organs.Balls.List.Sum(b => b.BaseValue) + organs.Dicks.List.Sum(d => d.BaseValue);
                    int femaleSum = organs.Vaginas.List.Sum(v => v.BaseValue) + organs.Boobs.List.Sum(b => b.BaseValue);
                    if (manSum < femaleSum)
                        partner.GainMasc(partner.LoseFemi(AmountToLose, changeLog) + bonus);
                    else
                        partner.GainFemi(partner.LoseMasc(AmountToLose, changeLog) + bonus);
                    break;
            }
            partner.InvokeUpdateAvatar();
        }

        private void MorphToDickgirl(BaseCharacter partner, ChangeLog changeLog)
        {
            partner.Essence.StableEssence.BaseValue += stableGain;
            if (partner.SexualOrgans.Vaginas.HaveAny())
            {
                int gain = partner.SexualOrgans.Vaginas.ReCycleOnce(changeLog);
                partner.Essence.Femininity.Amount += gain / 2;
                partner.Essence.Masculinity.Amount += gain / 2;
            }
            if (partner.SexualOrgans.Boobs.HaveAny())
                partner.SexualOrgans.Boobs.TryGrowSmallest(partner.Essence.Femininity);
            else
                partner.SexualOrgans.Boobs.TryGrowNew(partner.Essence.Femininity);
            partner.GrowOrgans();
        }

        private void MorphToCuntBoy(BaseCharacter partner, SexualOrgans organs, ChangeLog changeLog)
        {
            partner.Essence.StableEssence.BaseValue += stableGain;
            var toTakeFrom = new OrgansContainer[] { organs.Balls, organs.Dicks, organs.Boobs, };
            var withOrgans = toTakeFrom.Where(c => c.HaveAny()).ToArray();
            partner.Essence.Femininity.Amount += withOrgans[rng.Next(withOrgans.Length)].ReCycleOnce(changeLog);
            if (partner.SexualOrgans.Vaginas.HaveAny())
                partner.SexualOrgans.Vaginas.TryGrowSmallest(partner.Essence.Femininity);
            else
                partner.SexualOrgans.Vaginas.TryGrowNew(partner.Essence.Femininity);
            partner.GrowOrgans();
        }

        public enum MorphToGender
        {
            Disabled,
            Male,
            Female,
            CuntBoy,
            DickGirl,
            Futanari,
        }
    }
}