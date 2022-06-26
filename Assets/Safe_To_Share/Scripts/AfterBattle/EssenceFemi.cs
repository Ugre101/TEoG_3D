using Character;
using Character.EssenceStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle
{
    [CreateAssetMenu(fileName = "Drain Femi", menuName = "AfterBattle/Essence/Drain Femi")]
    public class EssenceFemi : EssenceAction
    {
        public override bool CanUse(BaseCharacter drainer, BaseCharacter victim)
            => victim.CanDrainFemi() && base.CanUse(drainer, victim);

        public override SexActData Use(AfterBattleActor drainer, AfterBattleActor victim)
        {
            victim.Actor.SexStats.UseSessionOrgasms(OrgasmsNeeded);
            foreach (EssencePerk perk in drainer.Actor.Essence.EssencePerks)
                perk.PerkDrainEssenceEffect(drainer.Actor, victim.Actor);
            ShowEffect(victim.transform);
            return SexActData(drainer.Actor.DrainFemi(victim.Actor));
        }
    }
}