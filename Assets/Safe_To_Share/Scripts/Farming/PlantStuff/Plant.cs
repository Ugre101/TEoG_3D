using System.Collections.Generic;
using UnityEngine;

namespace Safe_To_Share.Scripts.Farming
{
    [CreateAssetMenu(fileName = "Plant", menuName = "Plants/NewPlant", order = 0)]
    public class Plant : ScriptableObject
    {
        [field: SerializeField] public List<PlantDropItem> Drops { get; private set; } = new();
        
        
    }
}