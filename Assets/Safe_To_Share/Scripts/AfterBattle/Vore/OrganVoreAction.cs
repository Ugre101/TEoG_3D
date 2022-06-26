using System.Linq;
using Character;
using Character.Organs;
using Character.Organs.OrgansContainers;
using Character.VoreStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle.Vore
{
    [CreateAssetMenu(fileName = "Create OrganVoreAction", menuName = "Character/Vore/Vore Act", order = 0)]
    public class OrganVoreAction : VoreAction
    {
        [SerializeField] SexualOrganType organType;

        public override float ExtraCapacityNeeded(BaseCharacter pred, BaseCharacter prey)
        {
            if (!pred.SexualOrgans.Containers.TryGetValue(organType, out OrgansContainer container) ||
                !container.List.Any())
                return -1;
            return container.List.Min(organ => LacksCapacityBy(pred, prey, organ));
        }

        static float LacksCapacityBy(BaseCharacter pred, BaseCharacter prey, BaseOrgan organ)
            => prey.Body.Weight - (VoreSystemExtension.OrganVoreCapacity(pred, organ) -
                                   VoredCharacters.CurrentPreyTotalWeight(organ.Vore.PreysIds));

        public override bool CanUse(BaseCharacter giver, BaseCharacter receiver) =>
            giver.SexualOrgans.Containers.TryGetValue(organType, out OrgansContainer container) &&
            container.List.Count() != 0 &&
            container.List.Any(baseOrgan => VoreSystemExtension.CanOrganVore(giver, baseOrgan, receiver));

        public override SexActData Use(AfterBattleActor caster, AfterBattleActor target) =>
            caster.Actor.OrganVore(target.Actor, organType) ? data : errorData;
    }
}