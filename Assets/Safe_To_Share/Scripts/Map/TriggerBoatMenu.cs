using System;
using SceneStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.Map {
    public class TriggerBoatMenu : MonoBehaviour {
        [SerializeField] protected SceneTeleportExit exit;
        [SerializeField] Transform exitLocation;
        public Vector3 ExitLocation => exitLocation.position;

        protected virtual void Start() => exit.SetExit(ExitLocation);

        void OnDrawGizmosSelected() => Gizmos.DrawSphere(ExitLocation, 0.2f);

        void OnTriggerEnter(Collider other) {
            if (!other.CompareTag("Player"))
                return;
            OnPlayerEnter();
        }

        public static event Action OpenBoatMenu;

        protected virtual void OnPlayerEnter() => OpenBoatMenu?.Invoke();
    }
}