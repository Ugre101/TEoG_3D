using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Safe_To_Share.Scripts.Movement.HoverMovement
{
    [RequireComponent(typeof(Rigidbody),typeof(CharacterCapsule))]
    public abstract class MoveCharacter : MonoBehaviour
    {
        public enum MoveModes
        {
            Walking,
            Swimming,
        }

        [Header("Sprint Variables"), SerializeField, Range(1.15f, 2f),]
        float sprintMultiplier = 1.15f;

        [SerializeField] MoveModes currentMode;

        [FormerlySerializedAs("capsuleCollider"), SerializeField,]
        protected CharacterCapsule capsule;

        [field: SerializeField] public Rigidbody Rigid { get; private set; }
        [field: SerializeReference] public MoveStats Stats { get; private set; }
        protected float Speed => Stats.WalkSpeed;
        public float MaxSwimSpeed => Speed;

        public MoveModes CurrentMode
        {
            get => currentMode;
            protected set
            {
                currentMode = value;
                ChangedMode?.Invoke(value);
            }
        }

        protected float MaxSpeed => IsSprinting() ? Speed * Stats.SprintMultiplier : Speed;
        public bool Swimming => CurrentMode is MoveModes.Swimming;

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (Rigid == null)
            {
                if (TryGetComponent(out Rigidbody rb))
                    Rigid = rb;
                else
                    throw new MissingComponentException("Missing rigid body");
            }

            if (capsule == null && TryGetComponent(out capsule) is false)
                throw new MissingComponentException("Missing char capsule");

            if (Stats == null)
            {
                if (TryGetComponent(out MoveStats stats))
                    Stats = stats;
                else
                    throw new MissingComponentException("Missing move stats");
            }
        }
#endif
        public event Action<MoveModes> ChangedMode;

        public abstract bool IsCrouching();
        public abstract bool IsGrounded();
        public abstract bool IsSprinting();
        public abstract bool WasGrounded();
        public Vector3 GetVelocity() => Rigid.velocity;
        public float GetCurrentSpeed() => GetVelocity().magnitude;
        public float GetMaxSpeed() => MaxSpeed;
        public abstract Vector3 GetLocalMoveDirection();
        public abstract Vector3 GetUpVector();
        public abstract bool IsJumping();
    }
}