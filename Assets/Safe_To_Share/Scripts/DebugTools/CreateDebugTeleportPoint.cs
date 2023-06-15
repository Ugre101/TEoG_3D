using Unity.Mathematics;
using UnityEngine;

namespace Safe_To_Share.Scripts.DebugTools {
    public sealed class CreateDebugTeleportPoint : MonoBehaviour {
        [SerializeField] DebugTeleportPoint prefab;
        public LayerMask validRaycastTargets;

        public void AddNewPoint(Vector3 hitInfoPoint) =>
            Instantiate(prefab, hitInfoPoint, quaternion.identity, transform);
    }
}