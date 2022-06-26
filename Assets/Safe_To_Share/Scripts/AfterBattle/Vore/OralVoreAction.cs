using Character;
using Character.VoreStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle.Vore
{
    [CreateAssetMenu(menuName = "Create OralVoreAction", fileName = "Character/Vore/Vore Act", order = 0)]
    public class OralVoreAction : VoreAction
    {
        public override bool CanUse(BaseCharacter giver, BaseCharacter receiver) =>
            VoreSystemExtension.CanOralVore(giver, receiver);

        public override float ExtraCapacityNeeded(BaseCharacter pred, BaseCharacter prey) =>
            prey.Body.Weight - (VoreSystemExtension.OralVoreCapacity(pred) -
                                VoredCharacters.CurrentPreyTotalWeight(pred.Vore.Stomach.PreysIds));

        public override SexActData Use(AfterBattleActor caster, AfterBattleActor target)
        {
            if (!VoreSystemExtension.CanOralVore(caster.Actor, target.Actor))
                return errorData;
            caster.Actor.OralVore(target.Actor);
            return data;
        }
    }
}