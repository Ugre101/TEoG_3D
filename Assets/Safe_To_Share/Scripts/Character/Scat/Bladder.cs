using System;
using Safe_to_Share.Scripts.CustomClasses;
using UnityEngine;

namespace Safe_To_Share.Scripts.Character.Scat {
    [Serializable]
    public class Bladder {
        [SerializeField] float current;
        [field: SerializeField] public BaseConstFloatStat MaxPressure { get; private set; } = new(10);

        float Current => current;

        public float Empty() {
            var cur = Current;
            current = 0;
            BladderPressure?.Invoke(0);
            return cur;
        }

        public event Action<float> BladderPressure;
        public float Pressure() => Current / MaxPressure.Value;

        public void Fill(float amount) {
            current += amount;
            BladderPressure?.Invoke(Pressure());
        }
    }
}