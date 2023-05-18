using System;
using Safe_To_Share.Scripts.Movement.HoverMovement;
using UnityEngine;
using UnityEngine.AI;

namespace Safe_To_Share.Scripts.Movement.NavAgentMover
{
    public sealed class NavMover : MoveCharacter
    {
        [SerializeField] NavMeshAgent agent;


        [SerializeField] LayerMask groundLayer;


        void Start() => path = new NavMeshPath();

        public bool SampleAndSetPositionNear(Vector3 dest) => NavMesh.SamplePosition(dest, out var meshHit, 2f, groundLayer) && SetDestination(meshHit.position);

            NavMeshPath path;
        bool SetDestination(Vector3 dest)
        {
            if (!agent.CalculatePath(dest,path)) 
                return false;
            if (path.status != NavMeshPathStatus.PathComplete) 
                return false;
            agent.SetPath(path);
            return true;
        }

        public void SetSprint(bool sprint)
        {
            if (sprinting == sprint)
                return;
        
            if (sprint)
            {
                sprinting = true;
                agent.speed = Stats.WalkSpeed * Stats.SprintMultiplier;
                return;
            }

            sprinting = false;
            agent.speed = Stats.WalkSpeed;
        }

#if UNITY_EDITOR
        
        protected override void OnValidate()
        {
            base.OnValidate();
            if (agent == null && TryGetComponent(out agent) is false)
                throw new MissingComponentException("Missing nav agent");

        }
#endif

        public void SyncVariables()
        {
            agent.speed = Stats.WalkSpeed;

            agent.height = capsule.Height;
            agent.radius = capsule.Radius;
        }

        bool sprinting;
    
        public override bool IsCrouching() => false;

        public override bool IsGrounded() => true;

        public override bool IsSprinting() => sprinting;

        public override bool WasGrounded() => true;

        public override float GetCurrentSpeed() => base.GetCurrentSpeed();

        public override Vector3 GetLocalMoveDirection()
        {
            var dir = agent.nextPosition - agent.transform.position;
            dir.Normalize();
            return agent.transform.InverseTransformDirection(dir);
        }

        public override Vector3 GetUpVector() => agent.transform.rotation * Vector3.up;

        public override bool IsJumping() => false;
    }
}
