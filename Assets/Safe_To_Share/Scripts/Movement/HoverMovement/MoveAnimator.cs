using System;
using System.Collections;
using UnityEngine;

namespace Safe_To_Share.Scripts.Movement.HoverMovement {
    public sealed class MoveAnimator : MonoBehaviour {
        static readonly int Forward = Animator.StringToHash("Forward");
        static readonly int Turn = Animator.StringToHash("Turn");
        static readonly int Ground = Animator.StringToHash("OnGround");
        static readonly int Crouch = Animator.StringToHash("Crouch");
        static readonly int Jump = Animator.StringToHash("Jump");
        static readonly int Swimming = Animator.StringToHash("Swimming");

        [SerializeField] MoveCharacter character;
        //[SerializeField] AvatarChangerBase avatarChangerBase;

        [SerializeField] Animator animator;

        [SerializeField, Range(float.Epsilon, 0.1f),]
        float stopSwimmingDelay = 0.05f;

        bool isAnimatorNull;
        bool isCharacterNull;
        bool isSwimming;


        float LocalScaleX => transform.localScale.x;

        void Awake() {
            isCharacterNull = character == null;
            isAnimatorNull = animator == null;
        }

        void Update() {
            if (isCharacterNull || isAnimatorNull)
                return;

            var move = character.GetLocalMoveDirection();
            animator.SetBool(Crouch, character.IsCrouching());
            animator.SetBool(Ground, CheckGrounded());
            switch (character.CurrentMode) {
                case MoveCharacter.MoveModes.Walking:
                    if (character.IsGrounded()) {
                        Walking(move);
                        break;
                    }

                    var verticalSpeed = Vector3.Dot(character.GetVelocity(), character.GetUpVector());
                    animator.SetFloat(Jump, verticalSpeed, 0.3333f, Time.deltaTime);
                    break;
                case MoveCharacter.MoveModes.Swimming:
                    animator.SetBool(Crouch, false);
                    HandleSwimming(move);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

#if UNITY_EDITOR

        void OnValidate() {
            if (Application.isPlaying)
                return;
            //   if (animator == null && !TryGetComponent(out animator))
            //     throw new MissingComponentException();
            if (character == null && !transform.parent.TryGetComponent(out character))
                throw new MissingComponentException();
        }
#endif

        bool CheckGrounded() {
            if (character.IsGrounded())
                return true;
            if (character.WasGrounded())
                return !character.IsJumping();
            return character.Swimming;
        }

        public void SetAnimator(Animator obj) {
            animator = obj;
            isAnimatorNull = false;
        }

        void HandleSwimming(Vector3 move) {
            var forward = move.z;
            forward *= character.GetCurrentSpeed() / character.GetMaxSpeed(); //Mathf.Max(HeightSpeed(), float.Epsilon);
            StartSwimming();
            animator.SetFloat(Forward, forward);
            animator.SetFloat(Turn, move.x, 0.1f, Time.deltaTime);
        }

        void StartSwimming() {
            if (animator.GetBool(Swimming) is false)
                animator.SetBool(Swimming, true);
            isSwimming = true;
        }

        void Walking(Vector3 move) {
            var forward = move.z;
            forward *= character.GetCurrentSpeed() / Mathf.Max(HeightSpeed(), float.Epsilon);
            animator.SetFloat(Forward, forward, 0.1f, Time.deltaTime);
            animator.SetFloat(Turn, move.x, 0.1f, Time.deltaTime);
            if (isSwimming) {
                isSwimming = false;
                animator.SetBool(Swimming, false);
                //StartCoroutine(StopSwimmingSoon());
            }
        }

        // float realMaxSpeed = character.maxWalkSpeed * character.sprintSpeedMultiplier;
        float HeightSpeed() =>
            //if (character.CurrentMode == Movement.MoveModes.Walking && !character.IsSprinting() && !character.IsCrouching()) 
            //  return character.MaxWalkSpeed * Mathf.Clamp(character.SprintSpeedMultiplier, 1f, 1.2f) * GetHeightFactor();
            character.GetMaxSpeed() * GetHeightFactor();

        float GetHeightFactor() => LocalScaleX * 0.4f + 0.6f;

        IEnumerator StopSwimmingSoon() {
            float timePassed = 0;
            while (timePassed < stopSwimmingDelay) {
                if (isSwimming)
                    yield break;
                timePassed += Time.deltaTime;
                yield return null;
            }

            animator.SetBool(Swimming, false);
        }
    }
}