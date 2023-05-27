using System;
using UnityEngine;

namespace Safe_To_Share.Scripts.Movement.HoverMovement
{
    [Serializable]
    public struct FloatMod
    {
        public FloatMod(float value) => Value = value;
        [field: SerializeField] public float Value { get; private set; }
    }
}