using UnityEngine;

namespace Safe_To_Share.Scripts.Holders.UI
{
    public sealed class EnterBodyMorphPanel : MonoBehaviour
    {
        [SerializeField] BodyMorphPanel panel;

        void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") || !other.TryGetComponent(out PlayerHolder mover)) return;
            panel.Enter(mover);
            mover.PersonEcm2Character.Stop();
        }

        void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;
            panel.gameObject.SetActive(false);
        }
    }
}