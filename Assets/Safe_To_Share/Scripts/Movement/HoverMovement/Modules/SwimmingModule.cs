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
        [SerializeField, Range(float.Epsilon, 0.2f),] float swimDepthMargin = 0.1f;

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
            
            var yMin = SeaBottom();
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
            diveDepth = 0;
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
            if (StandingOnSeaBottom(out var diff))
            {
                // Debug.Log(diff);
                // TODO account for capsule height
                float test = capsule.Height;
                if (diff < 0)
                    offBy -= diff;   
            }
            var springForce = offBy * rideSpringStrength - relVel * RideSpringDamper;

            var force = rayDir * (springForce * rigid.mass);
            rigid.AddForce(force);

            if (hitOther)
                hitBody.AddForceAtPosition(rayDir * -springForce, checker.LastHit.point);
        }

        bool StandingOnSeaBottom(out float diff)
        {
            diff = capsule.BottomCenter.y - SeaBottom();
            if (capsule.YMax < waterLine)
                return false;
            return Mathf.Abs(diff) < 0.1f;
        }

        float OffFromTargetBy()
        {
            float test = capsule.Height * (swimAtPercentSubmerged + swimDepthMargin);
            var target = waterLine - test;  //(waterLine - capsule.Center.y) / capsule.Height;
            target = Mathf.Max(target - diveDepth, SeaBottom() + 0.1f);
            return capsule.YMin - target;
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

        public override void OnUpdateAvatarOffset()
        {
            
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