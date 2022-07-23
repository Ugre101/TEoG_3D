using UnityEngine;

namespace Movement.ECM2.Source.Components
{
    /// <summary>
    ///     Helper component used to define physics volumes like water, air, oil, etc.
    ///     Characters will react according to this settings when inside this volume.
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class PhysicsVolume : MonoBehaviour
    {
        #region FIELDS

        [SerializeField] BoxCollider _collider;

        #endregion

        #region EDITOR EXPOSED FIELDS

        [Tooltip("Determines which PhysicsVolume takes precedence if they overlap (higher value == higher priority)."),
         SerializeField,]
        int _priority;

        [Tooltip(
             "Determines the amount of friction applied by the volume as Character using CharacterMovement moves through it.\n" +
             "The higher this value, the harder it will feel to move through the volume."), SerializeField,]
        float _friction;

        [Tooltip("Determines the terminal velocity of Characters using CharacterMovement when falling."),
         SerializeField,]
        float _maxFallSpeed;

        [Tooltip("Determines if the volume contains a fluid, like water."), SerializeField,]
        bool _waterVolume;

        #endregion

        #region PROPERTIES

        bool hasCollider;
        /// <summary>
        ///     This volume collider (trigger).
        /// </summary>

        public BoxCollider boxCollider
        {
            get
            {
                if (!hasCollider && _collider == null)
                    _collider = GetComponent<BoxCollider>();
                hasCollider = true;
                return _collider;
            }
        }

        /// <summary>
        ///     Determines which PhysicsVolume takes precedence if they overlap (higher value == higher priority).
        /// </summary>

        public int priority
        {
            get => _priority;
            set => _priority = value;
        }

        /// <summary>
        ///     Determines the amount of friction applied by the volume as Character's using CharacterMovement move through it.
        ///     The higher this value, the harder it will feel to move through the volume.
        /// </summary>

        public float friction
        {
            get => _friction;
            set => _friction = Mathf.Max(0.0f, value);
        }

        /// <summary>
        ///     Determines the terminal velocity of Character's using CharacterMovement when falling.
        /// </summary>

        public float maxFallSpeed
        {
            get => _maxFallSpeed;
            set => _maxFallSpeed = Mathf.Max(0.0f, value);
        }

        /// <summary>
        ///     Determines if the volume contains a fluid, like water.
        /// </summary>

        public bool waterVolume
        {
            get => _waterVolume;
            set => _waterVolume = value;
        }

        #endregion

        #region METHODS

        protected virtual void OnReset()
        {
            priority = 0;
            friction = 0.5f;
            maxFallSpeed = 40.0f;
            waterVolume = true;
        }

        protected virtual void OnOnValidate()
        {
            friction = _friction;
            maxFallSpeed = _maxFallSpeed;
#if  UNITY_EDITOR
            if (TryGetComponent(out BoxCollider box))
                _collider = box;
#endif
        }

        protected virtual void OnAwake() => boxCollider.isTrigger = true;

        #endregion

        #region MONOBEHAVIOUR

        void Reset() => OnReset();

        void OnValidate() => OnOnValidate();

        void Awake() => OnAwake();

        #endregion
    }
}