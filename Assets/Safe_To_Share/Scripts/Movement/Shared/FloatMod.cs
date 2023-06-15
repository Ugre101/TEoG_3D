using System;
using UnityEngine;

namespace Safe_To_Share.Scripts.Movement.HoverMovement {
    [Serializable]
    public struct FloatMod {
        [field: SerializeField] public float Value { get; private set; }
        public FloatMod(float value) => Value = value;
    }
}