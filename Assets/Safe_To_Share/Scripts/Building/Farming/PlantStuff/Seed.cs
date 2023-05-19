using Items;
using UnityEngine;

namespace Safe_To_Share.Scripts.Farming
{
    [CreateAssetMenu(fileName = "Seed", menuName = "Plants/NewSeed", order = 0)]
    public sealed class Seed : Item
    {
       [field: SerializeField] public Plant SeedFor { get; private set; }
    }
}