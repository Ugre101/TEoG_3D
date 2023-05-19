using System.Collections.Generic;
using CustomClasses;
using UnityEngine;

namespace Safe_To_Share.Scripts.Farming
{
    [CreateAssetMenu(fileName = "Plant", menuName = "Plants/NewPlant", order = 0)]
    public sealed class Plant : SerializableScriptableObject
    {
        [field: SerializeField] public List<PlantDropItem> Drops { get; private set; } = new();
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public PlantedPlant Prefab { get; private set; }
        [field: SerializeField, Range(1, 99)] public int GrowTime { get; private set; } = 12;
    }
}