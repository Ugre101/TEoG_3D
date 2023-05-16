using Character;
using Character.EssenceStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle
{
    [CreateAssetMenu(fileName = "Give Femi", menuName = "AfterBattle/Essence/Give femi")]
    public sealed class GiveFemi : EssenceAction
    {
        public override bool CanUse(BaseCharacter drainer, BaseCharacter victim)
        {
            if (drainer.Essence.GiveAmount.Value <= 0) return false;
            if (drainer.Essence.EssenceOptions.SelfDrain)
            {
                if (!drainer.CanDrainFemi()) return false;
            }
            else if (drainer.Essence.Femininity.Amount <= 0) return false;

            return base.CanUse(drainer, victim);
        }

        public override SexActData Use(AfterBattleActor caster, AfterBattleActor target)
        {
            caster.Actor.SexStats.UseSessionOrgasms(OrgasmsNeeded);
            foreach (EssencePerk perk in caster.Actor.Essence.EssencePerks)
                perk.PerkGiveEssenceEffect(caster.Actor, target.Actor);
            return SexActData(caster.Actor.GiveFemi(target.Actor));
        }
    }
}