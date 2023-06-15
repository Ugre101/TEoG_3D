using UnityEngine;
using UnityEngine.AI;

namespace Safe_To_Share.Scripts.Holders.AI {
    public sealed class LocomotionSimpleAgent : MonoBehaviour {
        static readonly int Move = Animator.StringToHash("move");
        static readonly int Velx = Animator.StringToHash("Turn");
        static readonly int Vely = Animator.StringToHash("Forward");
        NavMeshAgent agent;
        Animator anim;
        LookAt lookAt;
        Vector2 smoothDeltaPosition = Vector2.zero;
        Vector2 velocity = Vector2.zero;

        void Start() {
            lookAt = GetComponent<LookAt>();
            anim = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            // Don’t update position automatically
            agent.updatePosition = false;
        }

        void Update() {
            var worldDeltaPosition = agent.nextPosition - transform.position;

            // Map 'worldDeltaPosition' to local space
            var dx = Vector3.Dot(transform.right, worldDeltaPosition);
            var dy = Vector3.Dot(transform.forward, worldDeltaPosition);
            Vector2 deltaPosition = new(dx, dy);

            // Low-pass filter the deltaMove
            var smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
            smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);
            // Update velocity if time advances
            if (Time.deltaTime > 1e-5f)
                velocity = smoothDeltaPosition / Time.deltaTime;

            var shouldMove = velocity.magnitude > 0.5f && agent.remainingDistance > agent.radius;

            // Update animation parameters
            anim.SetBool(Move, shouldMove);
            anim.SetFloat(Velx, velocity.x);
            anim.SetFloat(Vely, velocity.y);

            lookAt.lookAtTargetPosition = agent.steeringTarget + transform.forward;

            // Pull character towards agent
            if (worldDeltaPosition.magnitude > agent.radius)
                transform.position = agent.nextPosition - 0.9f * worldDeltaPosition;
        }

        void OnAnimatorMove() {
            // Update position based on animation movement using navigation surface height
            var position = anim.rootPosition;
            position.y = agent.nextPosition.y;
            transform.position = position;
        }
    }
}