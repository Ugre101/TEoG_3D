using System;
using UnityEngine;

namespace Safe_To_Share.Scripts.Movement.HoverMovement.Modules
{
    [Serializable]
    public abstract class BaseMoveModule
    {
        [SerializeField, Range(0f, 0.15f),] float brakeForce = 0.1f;
        protected CharacterCapsule capsule;
        protected GroundChecker checker;
        protected MoveInputs inputs;
        protected Transform offsetTransform;
        protected Rigidbody rigid;
        protected MoveStats stats;
        public abstract float MaxSpeed { get; }

        public bool IsCrouching { get; protected set; } = false;

        public virtual void OnStart(Rigidbody rb, CharacterCapsule cap, GroundChecker gc, MoveStats ms, MoveInputs mi,
                                    Transform avatarOffsetTransform)
        {
            rigid = rb;
            capsule = cap;
            checker = gc;
            stats = ms;
            inputs = mi;
            offsetTransform = avatarOffsetTransform;
        }

        public virtual void OnEnter(Collider collider)
        {
        }

        public virtual void OnExit()
        {
        }


        public abstract void OnGravity();

        public virtual void OnCapsuleSizeChange(float newHeight)
        {
        }

        public virtual void OnUpdateAvatarOffset()
        {
        }


        public abstract void OnMove(Vector3 force);

        public virtual void ApplyBraking()
        {
            var brakedVel = MovementTools.FlatVel(rigid.velocity) * (1f - brakeForce);
            brakedVel.y = rigid.velocity.y;
            rigid.velocity = brakedVel;
        }

        public virtual bool IsGrounded() => true;

        public virtual bool WarGrounded() => true;

        public virtual bool IsJumping() => false;
    }
}