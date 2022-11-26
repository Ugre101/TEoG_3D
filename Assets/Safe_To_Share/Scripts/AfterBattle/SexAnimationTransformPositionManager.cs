using System;
using System.Collections;
using AvatarStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle
{
    public class SexAnimationTransformPositionManager : MonoBehaviour
    {
        [SerializeField] float stepSize = 0.1f;
        [SerializeField] float timeBetweenSteps = 0.1f;
        bool a1Rotating;
        bool a2Rotating;
        bool raising;

        AfterBattleActor a1, a2;
        Quaternion a1StartRot;
        Quaternion a1TargetRot;
        float a1RotProgress;
        float a2RotProgress;

        public void PosActors(AfterBattleActor actor1,SexPositionPosAndRot pos1, AfterBattleActor actor2,SexPositionPosAndRot pos2)
        {
            a1 = actor1;
            a2 = actor2;
            var t1 = actor1.Avatar.KeyAreas.GetArea(pos1.KeyArea);
            var t2 = actor2.Avatar.KeyAreas.GetArea(pos2.KeyArea);
            if (t1 == null || t2 == null) return;
            float t1Y = t1.position.y;
            float t2Y = t2.position.y;

            HandleHeightDifference(actor1, actor2, t1Y, t2Y);
            actor1.RotateActor.SetFacingDirection(pos1.TowardsPartner);
            actor2.RotateActor.SetFacingDirection(pos2.TowardsPartner);
        }

        void HandleHeightDifference(AfterBattleActor actor1, AfterBattleActor actor2, float t1Y, float t2Y)
        {
            if (actor1.Avatar.transform.localScale.y > actor2.Avatar.transform.localScale.y)
            {
                if (t1Y > t2Y)
                    StartCoroutine(RaiseTowards(actor2, t1Y, t2Y));
            }
            else if (t2Y > t1Y)
                StartCoroutine(RaiseTowards(actor1, t2Y, t1Y));
        }

        void Update()
        {
            if (raising)
            {
                
            }
        }

      

        IEnumerator RaiseTowards(AfterBattleActor move, float from, float to)
        {
            float toMove = to - from;
            while (toMove > 0)
            {
                float step = Mathf.Min(toMove, stepSize);
                move.Avatar.transform.position += new Vector3(0, step, 0);
                toMove -= step;
                yield return new WaitForSeconds(timeBetweenSteps);
            }
        }
        
    }
}