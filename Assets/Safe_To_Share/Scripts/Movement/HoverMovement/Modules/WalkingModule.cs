using System;
using UnityEngine;

namespace Safe_To_Share.Scripts.Movement.HoverMovement.Modules
{
    [Serializable]
    public sealed class WalkingModule : BaseMoveModule
    {
        [SerializeField, Range(0.01f, 0.25f),] float hoverHeight = 2f;


        [SerializeField, Range(float.Epsilon, 99f),]
        float rideSpringStrength = 1;

        [SerializeField, Range(float.Epsilon, 1f),]
        float rideSpringDampFactor = 0.5f;

        [SerializeField, Range(0.1f, 0.5f),] float jumpCoolDown = 0.2f;
        [SerializeField, Range(1f, 2f),] float groundedPadding = 1.2f;

        [SerializeField, Range(15f, 90f),] float maxSlopeAngle = 45f;

        [SerializeField, Range(float.Epsilon, 1f),]
        float airMultiplier = 0.6f;

        [SerializeField, Range(float.Epsilon, 0.1f),]
        float updateAvatarOffsetFactor = float.Epsilon;


        bool isActiveTerrainNull;
        int jumps;

        float lastY;
        Vector3 startCenter;

        float targetHeight;
        float timeSinceLastJump;
        float updateAvatarOffsetTolerance = float.Epsilon;

        float RideSpringDamper => rideSpringStrength * rideSpringDampFactor;

        public override float MaxSpeed => stats.WalkSpeed;

        public override void OnEnter(Collider collider)
        {
            base.OnEnter(collider);
            startCenter = offsetTransform.localPosition;
            SetVariables();
            isActiveTerrainNull = Terrain.activeTerrain == null;
        }

        void SetVariables()
        {
            targetHeight = capsule.Height * hoverHeight;
            updateAvatarOffsetTolerance = updateAvatarOffsetFactor * capsule.Height;
        }

        public override void OnCapsuleSizeChange(float newHeight)
        {
            SetVariables();
        }

        public override void OnGravity()
        {
            if (IsGrounded() && checker.SlopeAngle() > maxSlopeAngle)
            {
                rigid.AddForce(Vector3.ProjectOnPlane(Physics.gravity, checker.LastHit.normal));
                return;
            }

            if (IsGrounded() is false || IsJumping())
            {
                rigid.AddForce(Physics.gravity);

                return;
            }

            var rayDir = rigid.transform.TransformDirection(Vector3.down);

            var hitOther = HitOther(rayDir, out var hitBody, out var otherDirVel);

            var rayDirVel = Vector3.Dot(rayDir, rigid.velocity);
            var relVel = rayDirVel - otherDirVel;

            var x = OffFromTargetBy();
            if (x < 0)
                x *= 2f;

            var springForce = x * rideSpringStrength - relVel * RideSpringDamper;

            var force = rayDir * (springForce * rigid.mass);
            rigid.AddForce(force);

            if (hitOther)
                hitBody.AddForceAtPosition(rayDir * -springForce, checker.LastHit.point);
        }

        bool HitOther(Vector3 rayDir, out Rigidbody hitBody, out float otherDirVel)
        {
            var hitOther = DidIHitOtherRigidBody(out hitBody);
            var otherVel = hitOther ? hitBody.velocity : Vector3.zero;
            otherDirVel = Vector3.Dot(rayDir, otherVel);
            return hitOther;

            bool DidIHitOtherRigidBody(out Rigidbody hitBody)
            {
                if (checker.LastHit.collider != null)
                    return checker.LastHit.collider.TryGetComponent(out hitBody);
                hitBody = default;
                return false;
            }
        }

        float OffFromTargetBy() => checker.DistanceToGround - targetHeight;

        public override bool IsGrounded() => checker.DistanceToGround < targetHeight * groundedPadding;


        public override bool IsJumping() => jumps > 0;

        

        public override void OnMove(Vector3 force)
        {
            var walkSpeed = force * (stats.WalkSpeed * rigid.mass);
            if (IsGrounded() && inputs.Sprinting)
                walkSpeed *= stats.SprintMultiplier;
            else if (!IsGrounded()) walkSpeed *= airMultiplier;

            if (StandingInSlope())
                rigid.AddForce(Vector3.ProjectOnPlane(walkSpeed, Vector3.down));
            else
                rigid.AddForce(walkSpeed, ForceMode.Force);
            HandleJumping();
            HandleCrunching();
        }

        void HandleCrunching()
        {
            var shouldCrunch = inputs.Crunching && IsGrounded();
            switch (shouldCrunch)
            {
                case true when IsCrouching is false:
                    StartCrunching();
                    break;
                case false when IsCrouching && CanStopCrunching():
                    StopCrunching();
                    break;
            }
        }

        public override void OnUpdateAvatarOffset()
        {
            if (checker.DidHitGround is false)
                return;
            if (IsJumping() || IsGrounded() is false)
                return;
            if (Math.Abs(checker.LastHit.point.y - lastY) < updateAvatarOffsetTolerance)
                return;
            var newPos = offsetTransform.position;
            newPos.y = checker.LastHit.point.y;
            offsetTransform.position = newPos;
            lastY = checker.LastHit.point.y;
        }

        void StopCrunching()
        {
            IsCrouching = false;
            capsule.RestoreHeight();
            StoppedCrunching?.Invoke();
        }

        public override void OnExit()
        {
            base.OnExit();
            offsetTransform.localPosition = startCenter;
            if (IsCrouching)
                StopCrunching();
        }

        bool StandingInSlope() => IsGrounded() && checker.SlopeAngle() > maxSlopeAngle;

        void HandleJumping()
        {
            UpdateJumpVariables();
            if (!inputs.Jumping || !CanJump())
                return;
            timeSinceLastJump = 0;
            jumps++;
            rigid.AddForce(stats.JumpStrength * rigid.mass * Vector3.up, ForceMode.Impulse);
        }


        void StartCrunching()
        {
            IsCrouching = true;
            var c1 = capsule.YMin;
            capsule.HalfHeight();
            var c2 = capsule.YMin;
            //  rigid.position += Vector3.down * (c2 - c1);
            StartedCrunching?.Invoke();
        }

        bool CanStopCrunching()
        {
            var capPos = capsule.Center;
            var offset = capsule.Height / 10;
            capPos.y -= offset;
            var ray = new Ray(capPos, Vector3.up);
            return !Physics.SphereCast(ray, capsule.Radius, capsule.Height);
        }

        void UpdateJumpVariables()
        {
            if (jumps > 0 && IsGrounded() && timeSinceLastJump > jumpCoolDown / 2f && !StandingInSlope())
                jumps = 0;
            timeSinceLastJump += Time.deltaTime;
        }

        bool CanJump() => jumps < stats.MaxJumpCount && jumpCoolDown < timeSinceLastJump;
        public event Action StartedCrunching;
        public event Action StoppedCrunching;
    }
}