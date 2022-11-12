using System;
using Safe_to_Share.Scripts.CustomClasses;
using UnityEngine;

namespace Safe_To_Share.Scripts.Character.Scat
{
    [Serializable]
    public class Bladder
    {
        [SerializeField] float current;
        [field: SerializeField] public BaseConstFloatStat MaxPressure { get; private set; } = new(10);

         float Current
        {
            get => current;
            set
            {
                current = Mathf.Clamp(current + value, 0, MaxPressure.Value);;
                BladderPressure?.Invoke(current);
            }
        }

        public event Action<float> BladderPressure;
        public float Pressure()
        {
            return Current / MaxPressure.Value;
        }
        public void Fill(float amount) => Current += amount;

        public float Relieve()
        {
            if (Current > 1)
            {
                Current -= 1;
                return 1f;
            }

            float have = Current;
            Current = 0;
            return have;
        }
    }
}