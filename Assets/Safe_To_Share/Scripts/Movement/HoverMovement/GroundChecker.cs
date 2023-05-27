using System;
using System.Collections.Generic;
using UnityEngine;

namespace Safe_To_Share.Scripts.Movement.HoverMovement
{
    public sealed class GroundChecker : MonoBehaviour
    {
        [SerializeField] LayerMask groundLayer;

        [SerializeField] CharacterCapsule capsule;

        public float DistanceToGround { get; private set; }

        public bool DidHitGround { get; private set; } = false;
        public RaycastHit LastHit { get; private set; }
        // Start is called before the first frame update

        public bool IsColliding { get; private set; }

        public readonly HashSet<ContactPoint> Collisions = new();
        void OnCollisionEnter(Collision collision)
        {
            Collisions.UnionWith(collision.contacts);
            IsColliding = Collisions.Count > 0;
        }
       public Vector3 HandleHittingWall(Vector3 walkSpeed)
        {
            var dir = Vector3.zero;
            foreach (var point in Collisions)
            {
                var temp = Vector3.Cross(point.normal, walkSpeed.normalized);
                dir += Vector3.Cross(temp, point.normal);
            }

            dir.Normalize();
            walkSpeed = Vector3.ProjectOnPlane(walkSpeed, dir);
            return walkSpeed;
        }
        void OnCollisionExit(Collision other)
        {
            Collisions.ExceptWith(other.contacts);
            IsColliding = Collisions.Count > 0;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(LastHit.point,capsule.Radius);
        }

        public void CheckGround()
        {
            if (!capsule.SphereCast(Vector3.down, 100f, groundLayer, out var hit))
            {
                DistanceToGround = 100f;
                DidHitGround = false;
                return;
            }

            LastHit = hit;
            DidHitGround = true;
            DistanceToGround = LastHit.distance - capsule.Height / 2f - capsule.Radius;
        }

        public float SlopeAngle() => Vector3.Angle(LastHit.normal, Vector3.up);

        public Vector3 SlopeDir(Vector3 moveDir)
            => Vector3.ProjectOnPlane(moveDir, LastHit.normal);
    }
}