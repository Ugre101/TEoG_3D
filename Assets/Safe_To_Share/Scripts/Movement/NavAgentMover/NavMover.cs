using Safe_To_Share.Scripts.Movement.HoverMovement;
using UnityEngine;
using UnityEngine.AI;

namespace Safe_To_Share.Scripts.Movement.NavAgentMover {
    public sealed class NavMover : MoveCharacter {
        [SerializeField] NavMeshAgent agent;


        [SerializeField] LayerMask groundLayer;

        NavMeshPath path;

        bool sprinting;


        void Start() => path = new NavMeshPath();

#if UNITY_EDITOR

        protected override void OnValidate() {
            base.OnValidate();
            if (agent == null && TryGetComponent(out agent) is false)
                throw new MissingComponentException("Missing nav agent");
        }
#endif

        public bool SampleAndSetPositionNear(Vector3 dest) =>
            NavMesh.SamplePosition(dest, out var meshHit, 2f, groundLayer) && SetDestination(meshHit.position);

        bool SetDestination(Vector3 dest) {
            if (!agent.CalculatePath(dest, path))
                return false;
            if (path.status != NavMeshPathStatus.PathComplete)
                return false;
            agent.SetPath(path);
            return true;
        }

        public void SetSprint(bool sprint) {
            if (sprinting == sprint)
                return;

            if (sprint) {
                sprinting = true;
                agent.speed = Stats.WalkSpeed * Stats.SprintMultiplier;
                return;
            }

            sprinting = false;
            agent.speed = Stats.WalkSpeed;
        }


        public void SyncVariables() {
            agent.speed = Stats.WalkSpeed;

            agent.height = capsule.Height;
            agent.radius = capsule.Radius;
        }

        public override bool IsCrouching() => false;

        public override bool IsGrounded() => true;

        public override bool IsSprinting() => sprinting;

        public override bool WasGrounded() => true;

        public override Vector3 GetVelocity() => agent.velocity;

        public override float GetCurrentSpeed() => base.GetCurrentSpeed();

        public override Vector3 GetLocalMoveDirection() {
            var test = agent.desiredVelocity;
            test = Vector3.ProjectOnPlane(test, GetUpVector());
            return agent.transform.InverseTransformDirection(test);
        }

        public override Vector3 GetUpVector() => agent.transform.rotation * Vector3.up;

        public override bool IsJumping() => false;
    }
}