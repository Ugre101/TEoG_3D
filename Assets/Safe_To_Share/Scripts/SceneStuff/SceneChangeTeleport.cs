using Safe_To_Share.Scripts.Holders;
using UnityEngine;

namespace SceneStuff {
    public sealed class SceneChangeTeleport : MonoBehaviour {
        [SerializeField] SceneTeleportExit exit;
        [SerializeField] LocationSceneSo sceneToLoad;
        [SerializeField] Transform exitLocation;

        bool alreadyLoading;
        Vector3 ExitLocation => exitLocation.position;

        void Start() => exit.SetExit(ExitLocation);

        void OnTriggerEnter(Collider other) {
            if (!other.gameObject.CompareTag("Player") || alreadyLoading || sceneToLoad == null)
                return;
            if (!other.TryGetComponent(out PlayerHolder player)) return;
            alreadyLoading = true;
            SceneLoader.Instance.LoadNewLocation(sceneToLoad, player.Player, exit);
        }
    }
}