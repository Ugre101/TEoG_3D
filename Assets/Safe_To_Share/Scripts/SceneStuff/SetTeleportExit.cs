using SceneStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.SceneStuff
{
    public class SetTeleportExit : MonoBehaviour
    {
        [SerializeField] SceneTeleportExit exit;
        void Start() => exit.SetExit(transform.position);

        void OnDrawGizmos() => Gizmos.DrawIcon(transform.position, "Teleport Exit");
    }
}