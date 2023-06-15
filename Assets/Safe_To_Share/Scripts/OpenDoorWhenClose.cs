using System.Collections;
using UnityEngine;

namespace Safe_To_Share.Scripts {
    public sealed class OpenDoorWhenClose : MonoBehaviour {
        [SerializeField] Animation openDoor;
        [SerializeField] OcclusionPortal occlusionPortal;
        readonly WaitForSeconds waitForSeconds = new(0.55f);

        void OnTriggerEnter(Collider other) {
            if (!other.CompareTag("Player")) return;
            openDoor.Play("Open_Door");
            openDoor.Rewind();
            occlusionPortal.open = true;
        }


        void OnTriggerExit(Collider other) {
            if (!other.CompareTag("Player")) return;
            openDoor.Rewind();
            openDoor.Play("Close_Door");
            StartCoroutine(ShortDelay());
        }

        IEnumerator ShortDelay() {
            yield return waitForSeconds;
            occlusionPortal.open = false;
        }
    }
}