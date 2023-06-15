using System.Collections;
using UnityEngine;

namespace Safe_To_Share.Scripts {
    public sealed class OpenAndCloserGate : MonoBehaviour {
        [SerializeField] Animation openGate;
        [SerializeField] OcclusionPortal occlusionPortal;
        readonly WaitForSeconds waitForSeconds = new(0.55f);

        void OnTriggerEnter(Collider other) {
            if (!other.CompareTag("Player")) return;
            openGate.Rewind();
            openGate.Play("Open");
            occlusionPortal.open = true;
        }

        void OnTriggerExit(Collider other) {
            if (!other.CompareTag("Player")) return;
            openGate.Rewind();
            openGate.Play("Close");
            StartCoroutine(ShortDelay());
        }

        IEnumerator ShortDelay() {
            yield return waitForSeconds;
            occlusionPortal.open = false;
        }
    }
}