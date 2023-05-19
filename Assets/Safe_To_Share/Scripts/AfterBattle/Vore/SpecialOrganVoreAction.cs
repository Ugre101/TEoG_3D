using System.Linq;
using Character;
using Character.Organs;
using Character.Organs.OrgansContainers;
using Character.VoreStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle.Vore
{
    [CreateAssetMenu(fileName = "New SpecialOrganVoreAction", menuName = "Character/Vore/Special Organ Vore Act")]
    public sealed class SpecialOrganVoreAction : VoreAction
    {
        [SerializeField] SexualOrganType organType;
        [SerializeField] SpecialVoreOptions specialDigestion;
        [SerializeField] bool onePreyOnly = true;

        public override float ExtraCapacityNeeded(BaseCharacter pred, BaseCharacter prey)
        {
            if (!pred.SexualOrgans.Containers.TryGetValue(organType, out BaseOrgansContainer container) ||
                !container.BaseList.Any())
                return -1;
            return container.BaseList.Min(organ => LacksCapacityBy(pred, prey, organ));
        }

        float LacksCapacityBy(BaseCharacter pred, BaseCharacter prey, BaseOrgan organ)
            => prey.Body.Weight - (VoreSystemExtension.OrganVoreCapacity(pred, organ, organType) -
                                   VoredCharacters.CurrentPreyTotalWeight(organ.Vore.PreysIds));

        public override bool CanUse(BaseCharacter giver, BaseCharacter receiver)
        {
            if (!giver.SexualOrgans.Containers.TryGetValue(organType, out BaseOrgansContainer container) ||
                !container.BaseList.Any())
                return false;
            if (needVorePerk.Any(needPerk => giver.Vore.Level.OwnedPerks.Any(p => p.Guid == needPerk.guid) == false))
                return false;
            return onePreyOnly
                ? container.BaseList.Any(o => o.Vore.SpecialPreysIds.Count == 0)
                : container.BaseList.Any(baseOrgan => VoreSystemExtension.CanOrganVore(giver, baseOrgan, receiver, organType));
        }

        public override SexActData Use(AfterBattleActor caster, AfterBattleActor target) =>
            caster.Actor.SpecialOrganVore(target.Actor, organType, specialDigestion, onePreyOnly) ? data : errorData;
    }
}