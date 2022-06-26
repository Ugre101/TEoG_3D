using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Safe_to_Share.Scripts.CustomClasses
{
    [Serializable]
    public class RngValue
    {
        [SerializeField] float minValue = 1, maxValue = 1;
        public float GetRandomValue => Random.Range(minValue, maxValue);
    }
}