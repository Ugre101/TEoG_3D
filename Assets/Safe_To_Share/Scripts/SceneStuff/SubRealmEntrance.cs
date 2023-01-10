using Safe_To_Share.Scripts.Holders;
using SaveStuff;
using SceneStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.SceneStuff
{
    public class SubRealmEntrance : MonoBehaviour
    {
        [SerializeField] SceneTeleportExit exit;
        [SerializeField] SubRealmSceneSo sceneToLoad;
        [SerializeField] Transform exitLocation;

        bool alreadyLoading;
        Vector3 ExitLocation => exitLocation.position;

        void Start() => exit.SetExit(ExitLocation);

        void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player") || alreadyLoading || sceneToLoad == null)
                return;
            if (!other.TryGetComponent(out PlayerHolder player)) return;
            alreadyLoading = true;
            SceneLoader.Instance.LoadSubRealm(sceneToLoad, player.Player, exit);
        }
    }
}