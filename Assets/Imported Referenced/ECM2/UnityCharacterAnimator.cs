using System;
using System.Collections;
using UnityEngine;

namespace Movement.ECM2.Source.Characters
{
    /// <summary>
    ///     This example shows how to externally animate a ECM2 character based on its current movement mode and / or state.
    /// </summary>
    public sealed class UnityCharacterAnimator : MonoBehaviour
    {
        static readonly int Forward = Animator.StringToHash("Forward");
        static readonly int Turn = Animator.StringToHash("Turn");
        static readonly int Ground = Animator.StringToHash("OnGround");
        static readonly int Crouch = Animator.StringToHash("Crouch");
        static readonly int Jump = Animator.StringToHash("Jump");
        static readonly int Swimming = Animator.StringToHash("Swimming");
        
        [SerializeField] ECM2Character character;
        [SerializeField] AvatarChangerBase avatarChangerBase;
        bool isCharacterNull;

        Animator animator;
        bool isAnimatorNull;
        bool isSwimming;

        void Awake()
        {
            isCharacterNull = character == null;
            isAnimatorNull = animator == null;
        }
        public void SetAnimator(Animator obj)
        {
            animator = obj;
            isAnimatorNull = false;
        }

        void Update()
        {
            if (isCharacterNull || isAnimatorNull)
                return;

            // Get Character animator

            // Compute input move vector in local space

            Vector3 move = transform.InverseTransformDirection(character.GetMovementDirection());
            bool backing = move.z < 0;
            // Update the animator parameters
            float forwardAmount = character.useRootMotion
                ? move.z
                : Mathf.InverseLerp(0.0f, HeightSpeed(), character.GetSpeed());
            animator.SetBool(Crouch, character.IsCrouching());
            animator.SetBool(Ground, character.IsOnGround() || character.WasOnGround());
            switch (character.GetMovementMode())
            {
                case MovementMode.None:
                    break;
                case MovementMode.Walking:
                    animator.SetFloat(Forward, backing ? -forwardAmount : forwardAmount, 0.1f, Time.deltaTime);
                    if (!backing)
                        animator.SetFloat(Turn, Mathf.Atan2(move.x, move.z), 0.1f, Time.deltaTime);
                    if (isSwimming)
                    {
                        isSwimming = false;
                        StartCoroutine(StopSwimmingSoon());
                    }
                    break;
                case MovementMode.Falling:
                    if (character.IsOnGround())
                        break;
                    float verticalSpeed = Vector3.Dot(character.GetVelocity(), character.GetUpVector());
                    animator.SetFloat(Jump, verticalSpeed, 0.3333f, Time.deltaTime);
                    break;
                case MovementMode.Swimming:
                    isSwimming = true;
                    animator.SetBool(Swimming,true);
                    // 0.5 => 1.3f
                    // 1 => 0.7f
                    // 3 => -1.5
                    float newOffset = 1.75f - transform.localScale.y;
                    float offSet = -0.45f + (2 - transform.localScale.y) * 1.2f; // TODO make exponental in both directions
                    transform.localPosition = new Vector3(0,newOffset,0);
                    break;
                case MovementMode.Flying:
                    break;
                case MovementMode.Custom:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // float realMaxSpeed = character.maxWalkSpeed * character.sprintSpeedMultiplier;
        float HeightSpeed() => character.GetMovementMode() == MovementMode.Walking && !character.IsSprinting() && !character.IsCrouching()
                ? character.maxWalkSpeed * Mathf.Clamp(character.sprintSpeedMultiplier, 1f, 1.2f) * (transform.localScale.x * 0.4f + 0.6f)
                : character.GetMaxSpeed() * (transform.localScale.x * 0.4f + 0.6f);

        IEnumerator StopSwimmingSoon()
        {
            float timePassed = 0;
            while (timePassed < 0.1f)
            {
                if (isSwimming)
                    yield break;
                timePassed += Time.deltaTime;
                yield return null;
            }
            animator.SetBool(Swimming,false);
            transform.localPosition = Vector3.zero;
        }
    }
}