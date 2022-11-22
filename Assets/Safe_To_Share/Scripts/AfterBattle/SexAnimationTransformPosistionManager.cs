using System.Collections;
using AvatarStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle
{
    public class SexAnimationTransformPosistionManager : MonoBehaviour
    {
        [SerializeField] float stepSize = 0.1f;
        [SerializeField] float timeBetweenSteps = 0.1f;
        public void PosActors(AfterBattleActor actor1,AvatarKeyAreas.Area area1, AfterBattleActor actor2,AvatarKeyAreas.Area area2)
        {
            var t1 = actor1.avatar.KeyAreas.GetArea(area1);
            var t2 = actor2.avatar.KeyAreas.GetArea(area2);
            if (t1 == null || t2 == null) return;
            float t1Y = t1.position.y;
            float t2Y = t2.position.y;
            if (actor1.avatar.transform.localScale.y > actor2.avatar.transform.localScale.y)
            {
                if (t1Y > t2Y) 
                    StartCoroutine(RaiseTowards(actor2, t1Y, t2Y));
            }
            else if (t2Y > t1Y) 
                StartCoroutine(RaiseTowards(actor1, t2Y, t1Y));
        }

        IEnumerator RaiseTowards(AfterBattleActor move, float from, float to)
        {
            float toMove = to - from;
            while (toMove > 0)
            {
                float step = Mathf.Min(toMove, stepSize);
                move.avatar.transform.position += new Vector3(0, step, 0);
                toMove -= step;
                yield return new WaitForSeconds(timeBetweenSteps);
            }
        }
        
    }
}