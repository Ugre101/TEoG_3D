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

        public float Pressure()
        {
            return current / MaxPressure.Value;
        }
        public void Fill(float amount)
        {
            current = Mathf.Clamp(current + amount, 0, MaxPressure.Value);
        }

        public float Relieve()
        {
            if (current > 1)
            {
                current -= 1;
                return 1f;
            }

            float have = current;
            current = 0;
            return have;
        }
    }
}