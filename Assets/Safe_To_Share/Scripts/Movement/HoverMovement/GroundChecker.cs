using System.Collections.Generic;
using UnityEngine;

namespace Safe_To_Share.Scripts.Movement.HoverMovement
{
    public sealed class GroundChecker : MonoBehaviour
    {
        [SerializeField] LayerMask groundLayer;

        [SerializeField] CharacterCapsule capsule;

        readonly HashSet<ContactPoint> collisions = new();

        RaycastHit[] results = new RaycastHit[16];

        public float DistanceToGround { get; private set; }

        public bool DidHitGround { get; private set; }

        public RaycastHit LastHit { get; private set; }
        // Start is called before the first frame update

        public bool IsColliding { get; private set; }

        void OnCollisionEnter(Collision collision)
        {
            collisions.UnionWith(collision.contacts);
            IsColliding = collisions.Count > 0;
        }

        void OnCollisionExit(Collision other)
        {
            collisions.ExceptWith(other.contacts);
            IsColliding = collisions.Count > 0;
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawSphere(LastHit.point, capsule.Radius);
        }

        public Vector3 HandleHittingWall(Vector3 walkSpeed)
        {
            var dir = Vector3.zero;
            foreach (var point in collisions)
            {
                var temp = Vector3.Cross(point.normal, walkSpeed.normalized);
                dir += Vector3.Cross(temp, point.normal);
            }

            dir.Normalize();
            walkSpeed = Vector3.ProjectOnPlane(walkSpeed, dir);
            return walkSpeed;
        }

        public void CheckGround()
        {
            var hits = capsule.SphereCastAllNonAlloc(Vector3.down, 100f, groundLayer, out results, 0.75f);
            if (hits < 1)
            {
                DistanceToGround = 100f;
                DidHitGround = false;
                return;
            }

            LastHit = GetGroundHit(hits);
            DidHitGround = true;
            DistanceToGround = LastHit.distance - capsule.Height / 2f - capsule.Radius;
        }

        RaycastHit GetGroundHit(int hits)
        {
            var closets = results[0];
            if (hits <= 1) return closets;

            for (var i = 1; i < hits; i++)
                if (results[i].distance < closets.distance)
                    closets = results[i];

            return closets;
        }

        public float SlopeAngle() => Vector3.Angle(LastHit.normal, Vector3.up);

        public Vector3 SlopeDir(Vector3 moveDir)
            => Vector3.ProjectOnPlane(moveDir, LastHit.normal);


        public bool CheckStuck(out Vector3 dir, float maxDist)
        {
            dir = Vector3.zero;
            if (IsColliding is false)
                return false;
            var penetrating = false;

            var pos = capsule.BottomCenter;
            foreach (var overLap in capsule.GetOverLapping(pos, groundLayer))
            {
                var otherTransform = overLap.transform;
                var penetration = Physics.ComputePenetration(capsule.Capsule, pos, capsule.transform.rotation, overLap,
                    otherTransform.position, otherTransform.rotation, out var tempDir, out var tempDist);
                if (penetration is false) continue;
                var distPush = Mathf.Min(maxDist, tempDist + float.Epsilon);
                var push = tempDir.normalized * distPush;
                penetrating = true;
                pos += push;
                dir += push;
            }

            return penetrating;
        }
    }
}