using Character;
using Character.EssenceStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle
{
    [CreateAssetMenu(fileName = "Drain Masc", menuName = "AfterBattle/Essence/Drain masc")]
    public class EssenceMasc : EssenceAction
    {
        public override bool CanUse(BaseCharacter drainer, BaseCharacter victim)
            => victim.CanDrainMasc() && base.CanUse(drainer, victim);

        public override SexActData Use(AfterBattleActor drainer, AfterBattleActor victim)
        {
            victim.Actor.SexStats.UseSessionOrgasms(OrgasmsNeeded);
            foreach (EssencePerk perk in drainer.Actor.Essence.EssencePerks)
                perk.PerkDrainEssenceEffect(drainer.Actor, victim.Actor);
            ShowEffect(victim.transform);
            return SexActData(drainer.Actor.DrainMasc(victim.Actor));
        }
    }
}