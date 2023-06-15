using System;
using Items;
using UnityEngine;

namespace Safe_To_Share.Scripts.Farming {
    [Serializable]
    public struct PlantDropItem {
        [field: SerializeField] public Item Item { get; private set; }
        [field: SerializeField, Range(0, 1f),] public float Chance { get; private set; }
    }
}