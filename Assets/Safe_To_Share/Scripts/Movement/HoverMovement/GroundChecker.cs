using UnityEngine;

namespace Safe_To_Share.Scripts.Movement.HoverMovement
{
    public sealed class GroundChecker : MonoBehaviour
    {
        [SerializeField] LayerMask groundLayer;

        [SerializeField] CharacterCapsule capsule;

        public float DistanceToGround { get; private set; }

        public bool DidHitGround { get; private set; } = false;
        public RaycastHit LastHit { get; private set; }
        // Start is called before the first frame update


        public void CheckGround()
        {
            if (!capsule.SphereCast(Vector3.down, 100f, groundLayer, out var hit))
            {
                DistanceToGround = 100f;
                DidHitGround = false;
                return;
            }

            LastHit = hit;
            DidHitGround = true;
            DistanceToGround = LastHit.distance - capsule.Height / 2f - capsule.Radius;
        }

        public float SlopeAngle() => Vector3.Angle(LastHit.normal, Vector3.up);

        public Vector3 SlopeDir(Vector3 moveDir)
            => Vector3.ProjectOnPlane(moveDir, LastHit.normal);
    }
}