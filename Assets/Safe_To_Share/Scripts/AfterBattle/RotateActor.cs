using System;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle
{
    public sealed class RotateActor : MonoBehaviour
    {
        ActorDirection facing;
        bool rotating;
        float rotProgress;
        Quaternion startQuaternion;
        Quaternion targetQuaternion;
        Vector3 startPos;
        Quaternion startRot;
        [SerializeField, HideInInspector,] Transform trans; // not sure if worth it but I did it to shut up rider.
#if UNITY_EDITOR
        void OnValidate()
        {
            if (trans == null)
                trans = transform;
        }
#endif

        void Start()
        {
            startPos = trans.position;
            startRot = trans.rotation;
        }

        public void SetFacingDirection(ActorDirection direction)
        {
            if (facing == direction) return;
            facing = direction;
            ResetPosAndRot();
            rotProgress = 0;
            var target = trans.eulerAngles;
            switch (direction)
            {
                case ActorDirection.Front: // Same as reset pos
                    break;
                case ActorDirection.Back:
                    target.y += 180f;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }

            startQuaternion = trans.rotation;
            targetQuaternion = Quaternion.Euler(target);
            rotating = true;
        }
        public void ResetPosAndRot() => trans.SetPositionAndRotation(startPos, startRot);

        public void Update()
        {
            if (!rotating) return;
            if (rotProgress < 1)
            {
                rotProgress += Time.deltaTime * 3;
                trans.rotation =
                    Quaternion.Lerp(startQuaternion, targetQuaternion, rotProgress);
            }
            else
                rotating = false;
        }
    }
}