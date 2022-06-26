using AvatarStuff.Holders;
using Character.PlayerStuff;
using SaveStuff;
using Static;
using UnityEngine;

namespace SceneStuff
{
    public class SceneChangeTeleport : MonoBehaviour
    {
        [SerializeField] SceneTeleportExit exit;
        [SerializeField] LocationSceneSo sceneToLoad;
        [SerializeField] Transform exitLocation;

        bool alreadyLoading;
        public Vector3 ExitLocation => exitLocation.position;

        void Start() => exit.SetExit(ExitLocation);

        void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player") || alreadyLoading || sceneToLoad == null)
                return;
            if (other.TryGetComponent(out PlayerHolder player))
            {
                alreadyLoading = true;
                SceneLoader.Instance.LoadNewLocation(sceneToLoad, player.Player, exit);
            }
        }
    }
}