using System.Collections;
using Safe_To_Share.Scripts.AfterBattle.Animation;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle {
    public sealed class SexAnimationTransformPositionManager : MonoBehaviour {
        public void PosActors(AfterBattleActor playerControlled, AfterBattleActor partner, SexActionAnimation ani) {
            if (!playerControlled.HasAvatar || !partner.HasAvatar)
                return;
            var t1 = playerControlled.Avatar.KeyAreas.GetArea(ani.GivePos.KeyArea);
            var t2 = partner.Avatar.KeyAreas.GetArea(ani.ReceivePos.KeyArea);
            if (t1 == null || t2 == null) return;


            if (ani.ReceivePos.SetAsChild)
                SetAsChild(partner, t1, t2, ani.ReceivePos);

            if (ani.GivePos.SetAsChild)
                SetAsChild(playerControlled, t2, t1, ani.GivePos);

            if (playerControlled.AvatarScaler.Height < partner.AvatarScaler.Height)
                playerControlled.AlignActor.AlignWith(t1, t2, ani.StayGrounded);
            else
                partner.AlignActor.AlignWith(t2, t1, ani.StayGrounded);

            playerControlled.RotateActor.SetFacingDirection(ani.GivePos.TowardsPartner);
            partner.RotateActor.SetFacingDirection(ani.ReceivePos.TowardsPartner);
        }

        void SetAsChild(AfterBattleActor child, Transform t1, Transform t2, SexPositionPosAndRot posAndRot) {
            if (posAndRot.AfterDelay > 0) {
                StartCoroutine(WaitAndSetAsChild(posAndRot.AfterDelay, t1, t2, child, posAndRot.Rotation,
                    posAndRot.Offset));
            } else {
                child.transform.SetParent(t1);
                child.transform.localPosition = Vector3.zero;
            }
        }

        static IEnumerator WaitAndSetAsChild(float receivePosAfterDelay, Transform area, Transform ownArea,
                                             AfterBattleActor actor, Vector3 rot, Vector3 offset) {
            yield return new WaitForSeconds(receivePosAfterDelay);
            actor.transform.SetParent(area);
            actor.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(rot));
            var diff = ownArea.position - area.position;
            actor.transform.position -= diff;
            actor.transform.position += offset;
        }
    }
}