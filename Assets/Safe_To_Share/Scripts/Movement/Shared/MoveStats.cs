using System;
using UnityEngine;

namespace Safe_To_Share.Scripts.Movement.HoverMovement
{
    [Serializable]
    public abstract class MoveStats : MonoBehaviour
    {
        public abstract int MaxJumpCount { get; }
        public abstract float JumpStrength { get; }
        public abstract float SprintMultiplier { get; }
        public abstract float SwimSpeed { get; }
        public abstract float WalkSpeed { get; }

        public abstract void AddMod(MoveCharacter.MoveModes walking, FloatMod speedMod);
        public abstract void RemoveMod(MoveCharacter.MoveModes walking, FloatMod speedMod);
    }
}