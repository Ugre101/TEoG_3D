using System;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle.Actor
{
    public class AlignActors : MonoBehaviour
    {
        [SerializeField, HideInInspector,] Transform trans; // not sure if worth it but I did it to shut up rider.
        bool moving;
        Transform toAlign, target;
        [SerializeField, Range(float.Epsilon, 1f)] float maxDelta = 0.1f;
        [SerializeField,Range(float.Epsilon,1f)] float tolerence = 0.5f;
#if UNITY_EDITOR
        void OnValidate()
        {
            if (trans == null)
                trans = transform;
        }
#endif

        public void AlignWith(Transform mine,Transform theirs)
        {
            if (theirs == null ||mine == null) return;
            moving = true;
            toAlign = mine;
            target = theirs;
        }
        void Update()
        {
            if (!moving) return;
            var delta = maxDelta * Time.deltaTime;
            var diff = toAlign.position - target.position;
            if (diff.y < -tolerence)
                trans.position += new Vector3(0, delta, 0);
            else if (diff.y > tolerence)
                trans.position -= new Vector3(0, delta, 0);
            else moving = false;
        }
    }
}