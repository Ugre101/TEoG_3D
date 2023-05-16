using Character;
using Character.EssenceStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle
{
    [CreateAssetMenu(fileName = "Give Masc", menuName = "AfterBattle/Essence/Give masc")]
    public sealed class GiveMasc : EssenceAction
    {
        public override bool CanUse(BaseCharacter drainer, BaseCharacter victim)
        {
            if (drainer.Essence.GiveAmount.Value <= 0) return false;
            if (drainer.Essence.EssenceOptions.SelfDrain)
            {
                if (!drainer.CanDrainMasc()) return false;
            }
            else if (drainer.Essence.Masculinity.Amount <= 0) return false;

            return base.CanUse(drainer, victim);
        }

        public override SexActData Use(AfterBattleActor caster, AfterBattleActor target)
        {
            caster.Actor.SexStats.UseSessionOrgasms(OrgasmsNeeded);
            foreach (EssencePerk perk in caster.Actor.Essence.EssencePerks)
                perk.PerkGiveEssenceEffect(caster.Actor, target.Actor);
            return SexActData(caster.Actor.GiveMasc(target.Actor));
        }
    }
}