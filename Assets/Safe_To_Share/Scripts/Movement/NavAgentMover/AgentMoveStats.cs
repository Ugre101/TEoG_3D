using Safe_To_Share.Scripts.Movement.HoverMovement;
using UnityEngine;

namespace Safe_To_Share.Scripts.Movement.NavAgentMover
{
    public sealed class AgentMoveStats : MoveStats
    {
        [SerializeField, Range(1.1f,2f),] float sprintMultiplier = 1.2f;
        [SerializeField, Range(6f,20f),] float swimSpeed = 8;
        [SerializeField, Range(6f,20f),] float walkSpeed = 10;
        public override int MaxJumpCount => 0;
        public override float JumpStrength => 0;

        public override float SprintMultiplier => sprintMultiplier;

        public override float SwimSpeed => swimSpeed;

        public override float WalkSpeed => walkSpeed;

        public override void AddMod(MoveCharacter.MoveModes walking, FloatMod speedMod)
        {
        }

        public override void RemoveMod(MoveCharacter.MoveModes walking, FloatMod speedMod)
        {
        }
    }
}