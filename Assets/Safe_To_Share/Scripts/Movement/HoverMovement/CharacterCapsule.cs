using UnityEngine;

namespace Safe_To_Share.Scripts.Movement.HoverMovement
{
    [RequireComponent(typeof(CapsuleCollider))]
    public sealed class CharacterCapsule : MonoBehaviour
    {
        [SerializeField] CapsuleCollider capsule;

        [SerializeField, HideInInspector,] Vector3 p1PreCalcMath;
        [SerializeField, HideInInspector,] Vector3 p2PreCalcMath;

        [SerializeField, HideInInspector,] float radiusRatio = 0.25f;

        bool crunching;

        float currentHeight;

        public CapsuleCollider Capsule => capsule;

        public Vector3 P1PreCalcMath => p1PreCalcMath;

        public Vector3 P2PreCalcMath => p2PreCalcMath;
        public float Radius => capsule.radius;

        public Vector3 BottomCenter
        {
            get
            {
                var bounds = capsule.bounds;
                var center = bounds.center;
                center.y = bounds.min.y;
                return center;
            }
        }

        public Vector3 Center => Capsule.bounds.center;

        public float Height => capsule.height;
        public float YMax => Capsule.bounds.max.y;
        public float YMin => Capsule.bounds.min.y;

        void Start()
        {
            currentHeight = capsule.height;
            CalcCapsulePosition();
        }

#if UNITY_EDITOR

        void OnValidate()
        {
            if (Application.isPlaying)
                return;
            if (capsule == null && TryGetComponent(out capsule) is false)
                throw new MissingComponentException("Missing capsule");
            radiusRatio = capsule.radius / capsule.height;
        }
#endif

        void CalcCapsulePosition()
        {
            p1PreCalcMath = Vector3.up * (capsule.radius + Physics.defaultContactOffset);
            p2PreCalcMath = Vector3.up * (capsule.height - capsule.radius + Physics.defaultContactOffset);
        }


        public void SetHeight(float height)
        {
            currentHeight = height;
            capsule.height = currentHeight;
            capsule.radius = currentHeight * radiusRatio;
            if (crunching)
            {
                HalfHeight();
            }
            else
            {
                CalcCapsuleCenter();
                CalcCapsulePosition();
            }
        }

        void CalcCapsuleCenter() => capsule.center = currentHeight * 0.5f * Vector3.up;

        public void HalfHeight()
        {
            capsule.height /= 2f;
            CalcCapsuleCenter();
            CalcCapsulePosition();
            crunching = true;
        }

        public void RestoreHeight()
        {
            capsule.height = currentHeight;
            CalcCapsuleCenter();
            CalcCapsulePosition();
            crunching = false;
        }

        public bool SphereCast(Vector3 dir, float distance, LayerMask layerMask, out RaycastHit hit) =>
            Physics.SphereCast(Center, Radius, dir, out hit, distance, layerMask,QueryTriggerInteraction.Ignore);

        public SpherecastCommand SphereCheck(Vector3 direction, float distance, LayerMask layerMask) =>
            new(Center, Radius, direction, distance, layerMask);

        public int OverlapCapsuleNonAlloc(Vector3 capsuleBottom, Collider[] results, LayerMask groundLayer)
            => Physics.OverlapCapsuleNonAlloc(capsuleBottom + P1PreCalcMath, capsuleBottom + P2PreCalcMath,
                Radius, results, groundLayer);
    }
}