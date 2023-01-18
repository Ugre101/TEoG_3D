using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle
{
    public class SexAnimationTransformPositionManager : MonoBehaviour
    {
        public void PosActors(AfterBattleActor playerControlled, SexPositionPosAndRot pos1, AfterBattleActor partner,
                              SexPositionPosAndRot pos2, bool stayGrounded)
        {
            var t1 = playerControlled.Avatar.KeyAreas.GetArea(pos1.KeyArea);
            var t2 = partner.Avatar.KeyAreas.GetArea(pos2.KeyArea);
            if (t1 == null || t2 == null) return;

            HandleHeightDifference(playerControlled, partner, t1, t2, stayGrounded);
            playerControlled.RotateActor.SetFacingDirection(pos1.TowardsPartner);
            partner.RotateActor.SetFacingDirection(pos2.TowardsPartner);
        }

        static void HandleHeightDifference(AfterBattleActor actor1, AfterBattleActor partnerActor, Transform player,
                                           Transform partner, bool stayGrounded)
        {
            if (actor1.AvatarScaler.Height < partnerActor.AvatarScaler.Height)
                actor1.AlignActor.AlignWith(player, partner, stayGrounded);
            else
                partnerActor.AlignActor.AlignWith(partner, player, stayGrounded);
        }
    }
}