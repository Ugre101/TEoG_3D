using System.Collections;
using Safe_To_Share.Scripts.AfterBattle.Animation;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle
{
    public class SexAnimationTransformPositionManager : MonoBehaviour
    {
        public void PosActors(AfterBattleActor playerControlled, AfterBattleActor partner, SexActionAnimation ani)
        {
            var t1 = playerControlled.Avatar.KeyAreas.GetArea(ani.GivePos.KeyArea);
            var t2 = partner.Avatar.KeyAreas.GetArea(ani.ReceivePos.KeyArea);
            if (t1 == null || t2 == null) return;

            if (ani.ReceivePos.SetAsChild)
            {
                if (ani.ReceivePos.AfterDelay > 0)
                    StartCoroutine(WaitAndSetAsChild(ani.ReceivePos.AfterDelay, t1, t2, partner,ani.ReceivePos.Rotation,ani.ReceivePos.Offset));
                else
                {
                    partner.transform.SetParent(t1);
                    partner.transform.localPosition = Vector3.zero;
                }
            }

            if (ani.GivePos.SetAsChild)
            {
                if (ani.GivePos.AfterDelay > 0)
                    StartCoroutine(WaitAndSetAsChild(ani.GivePos.AfterDelay, t2,t1, playerControlled,ani.GivePos.Rotation,ani.GivePos.Offset));
                else
                {
                    playerControlled.transform.SetParent(t2);
                    playerControlled.transform.localPosition = Vector3.zero;
                }
            }

            if (playerControlled.AvatarScaler.Height < partner.AvatarScaler.Height)
                playerControlled.AlignActor.AlignWith(t1, t2, ani.StayGrounded);
            else
                partner.AlignActor.AlignWith(t2, t1, ani.StayGrounded);

            playerControlled.RotateActor.SetFacingDirection(ani.GivePos.TowardsPartner);
            partner.RotateActor.SetFacingDirection(ani.ReceivePos.TowardsPartner);
        }

        IEnumerator WaitAndSetAsChild(float receivePosAfterDelay, Transform area, Transform ownArea,
                                      AfterBattleActor actor, Vector3 rot, Vector3 offset)
        {
            yield return new WaitForSeconds(receivePosAfterDelay);
            actor.transform.SetParent(area);
            actor.transform.localPosition = Vector3.zero;
            actor.transform.localEulerAngles = rot;
            var diff = ownArea.position - area.position;
            actor.transform.position -= diff;
            actor.transform.position += offset;
        }
    }
}