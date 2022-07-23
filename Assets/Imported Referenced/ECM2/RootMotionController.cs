using System;
using Movement.ECM2.Source;
using UnityEngine;

namespace ECM2.Source.Helpers
{
    /// <summary>
    ///     RootMotionController.
    ///     Helper component to get Animator root motion velocity vector (animRootMotionVelocity).
    ///     This must be attached to a game object with an Animator component.
    /// </summary>
   //[RequireComponent(typeof(Animator))]
    public sealed class RootMotionController : MonoBehaviour
    {
        #region FIELDS

        [SerializeField] AvatarChangerBase avatarChangerBase;
        Animator animator;

        #endregion

        #region METHOD

        /// <summary>
        ///     Calculate velocity from anim root motion.
        /// </summary>
        Vector3 CalcAnimRootMotionVelocity()
        {
            float deltaTime = Time.deltaTime;

            return deltaTime > 0.0f ? animator.deltaPosition / deltaTime : Vector3.zero;
        }

        #endregion

        #region PROPERTIES

        /// <summary>
        ///     The animation root motion velocity vector.
        /// </summary>

        public Vector3 AnimRootMotionVelocity { get; private set; }

        /// <summary>
        ///     The animation root motion delta rotation.
        /// </summary>

        public Quaternion AnimDeltaRotation => animator.deltaRotation;

        #endregion

        #region MONOBEHAVIOUR


        public void SetAnimator(Animator obj) => animator = obj;

        public void OnAnimatorMove() => // Compute animation root motion velocity
            AnimRootMotionVelocity = CalcAnimRootMotionVelocity();

        #endregion

#if UNITY_EDITOR
        void OnValidate()
        {
            if (avatarChangerBase != null) return;
            if (TryGetComponent(out AvatarChangerBase changerBase))
                avatarChangerBase = changerBase;
            else
                Debug.LogWarning("Has no avatarChanger",this);
        }
#endif
    }
}