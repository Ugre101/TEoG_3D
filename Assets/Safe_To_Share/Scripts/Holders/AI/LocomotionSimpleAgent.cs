using UnityEngine;
using UnityEngine.AI;

namespace AvatarStuff.Holders.AI
{
    public class LocomotionSimpleAgent : MonoBehaviour
    {
        static readonly int Move = Animator.StringToHash("move");
        static readonly int Velx = Animator.StringToHash("Turn");
        static readonly int Vely = Animator.StringToHash("Forward");
        NavMeshAgent agent;
        Animator anim;
        LookAt lookAt;
        Vector2 smoothDeltaPosition = Vector2.zero;
        Vector2 velocity = Vector2.zero;

        void Start()
        {
            lookAt = GetComponent<LookAt>();
            anim = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            // Don’t update position automatically
            agent.updatePosition = false;
        }

        void Update()
        {
            Vector3 worldDeltaPosition = agent.nextPosition - transform.position;

            // Map 'worldDeltaPosition' to local space
            float dx = Vector3.Dot(transform.right, worldDeltaPosition);
            float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
            Vector2 deltaPosition = new(dx, dy);

            // Low-pass filter the deltaMove
            float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
            smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);
            // Update velocity if time advances
            if (Time.deltaTime > 1e-5f)
                velocity = smoothDeltaPosition / Time.deltaTime;

            bool shouldMove = velocity.magnitude > 0.5f && agent.remainingDistance > agent.radius;

            // Update animation parameters
            anim.SetBool(Move, shouldMove);
            anim.SetFloat(Velx, velocity.x);
            anim.SetFloat(Vely, velocity.y);

            lookAt.lookAtTargetPosition = agent.steeringTarget + transform.forward;

            // Pull character towards agent
            if (worldDeltaPosition.magnitude > agent.radius)
                transform.position = agent.nextPosition - 0.9f * worldDeltaPosition;
        }

        void OnAnimatorMove()
        {
            // Update position based on animation movement using navigation surface height
            Vector3 position = anim.rootPosition;
            position.y = agent.nextPosition.y;
            transform.position = position;
        }
    }
}