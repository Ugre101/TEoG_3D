using System;
using UnityEngine;

namespace Safe_To_Share.Scripts.Movement.HoverMovement.Modules
{
    [Serializable]
    public sealed class SwimmingModule : BaseMoveModule
    {
        [SerializeField, Range(float.Epsilon, 99f),] float rideSpringStrength = 1;

        [SerializeField, Range(float.Epsilon, 1f),] float rideSpringDampFactor = 0.5f;

        [SerializeField, Range(float.Epsilon, 1f),] float swimAtPercentSubmerged = 0.5f;

        float diveDepth;

        Collider water;
        float waterLine;
        public override float MaxSpeed => stats.SwimSpeed;
        float RideSpringDamper => rideSpringStrength * rideSpringDampFactor;
        public bool ShouldSwim(Collider other)
        {
            waterLine = other.bounds.max.y;
            var bounds = capsule.Capsule.bounds;
            var yMax = bounds.max.y;
            if (yMax < waterLine)
                return true;
            
            var yMin = bounds.min.y;
            if (waterLine < yMin) 
                return false;
            
            var submerged = waterLine - yMin;
            var percentSubmerged = submerged / capsule.Height;
            return swimAtPercentSubmerged < percentSubmerged;
        }
        public override void OnEnter(Collider collider)
        {
            base.OnEnter(collider);
            water = collider;
            waterLine = water.bounds.max.y;
            offsetTransform.localPosition = Vector3.zero;
        }

        public override void OnGravity()
        {
            var rayDir = rigid.transform.TransformDirection(Vector3.down);

            var hitOther = DidIHitOtherRigidBody(out var hitBody);
            var otherVel = hitOther ? hitBody.velocity : Vector3.zero;
            var otherDirVel = Vector3.Dot(rayDir, otherVel);

            var rayDirVel = Vector3.Dot(rayDir, rigid.velocity);
            var relVel = rayDirVel - otherDirVel;

            var offBy = OffFromTargetBy();

            var springForce = offBy * rideSpringStrength - relVel * RideSpringDamper;

            var force = rayDir * (springForce * rigid.mass);
            rigid.AddForce(force);

            if (hitOther)
                hitBody.AddForceAtPosition(rayDir * -springForce, checker.LastHit.point);
        }

        float OffFromTargetBy()
        {
            var target = waterLine - (waterLine - capsule.Capsule.bounds.min.y) / capsule.Height;
            target = Mathf.Max(target - diveDepth, SeaBottom() + 0.1f);

            return capsule.Center.y - target;
        }

        float SeaBottom() => checker.LastHit.point.y;

        bool DidIHitOtherRigidBody(out Rigidbody hitBody)
        {
            if (checker.LastHit.collider != null)
                return checker.LastHit.collider.TryGetComponent(out hitBody);
            hitBody = default;
            return false;
        }

        public override void OnMove(Vector3 force)
        {
            var swimSpeed = force * (stats.SwimSpeed * rigid.mass);
            if (inputs.Sprinting)
                swimSpeed *= stats.SprintMultiplier;
            rigid.AddForce(swimSpeed, ForceMode.Force);
            HandleDiving();
        }

        void HandleDiving()
        {
            if (inputs.Crunching)
            {
                if (waterLine - diveDepth < SeaBottom())
                    return;
                diveDepth += 0.1f;
                return;
            }

            if (inputs.Jumping && diveDepth > 0)
                diveDepth -= 0.2f;
            else if (diveDepth > 0)
                diveDepth -= 0.01f;

            if (diveDepth < 0)
                diveDepth = 0;
        }

        public override bool IsGrounded() => true;
    }
}