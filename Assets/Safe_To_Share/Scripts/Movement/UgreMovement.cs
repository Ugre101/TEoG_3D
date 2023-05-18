using System;
using UnityEngine;

namespace Safe_To_Share.Scripts.Movement
{
    public sealed class UgreMovement : MonoBehaviour
    {
        [SerializeField] float baseSpeed;
        [SerializeField] float sprintMultiplier;

        Vector3 moveDir;
        Rigidbody rb;
        Transform orientation;

        Vector2 input;
        
        void Move()
        {
            moveDir = orientation.forward * input.y + orientation.right * input.x;
            
            rb.AddForce(moveDir.normalized * baseSpeed,ForceMode.Force);
        }
    }

    public sealed class JumpManager : MonoBehaviour
    {
        [SerializeField] GroundChecker groundChecker;

        int currentJumps;
        int maxJumps;

        bool wantToJump;
        bool haveJumped;
        void FixedUpdate()
        {
            if (InWater()) 
                return;
            if (wantToJump && currentJumps < maxJumps)
            {
                Jump();
                return;
            }
            if (haveJumped && groundChecker.IsGrounded())
            {
            }
        }

        void Jump()
        {
            haveJumped = true;
            throw new NotImplementedException();
        }

        bool InWater()
        {
            throw new NotImplementedException();
        }
    }
    
    public class GroundChecker : MonoBehaviour
    {
        public bool IsGrounded()
        {
            return false;
        }
    }

    public class UgreMovementAnimator : MonoBehaviour
    {
        
    }
}