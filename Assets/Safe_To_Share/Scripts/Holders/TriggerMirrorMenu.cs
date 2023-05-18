using UnityEngine;

namespace Safe_To_Share.Scripts.Holders
{
    public sealed class TriggerMirrorMenu : MonoBehaviour
    {
        [SerializeField] ChangeAvatarDetails mirrorUI;

        void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") || !other.TryGetComponent(out PlayerHolder mover)) 
                return;
            mirrorUI.Enter(mover);
            mover.PersonEcm2Character.Stop();
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
                mirrorUI.gameObject.SetActive(false);
        }
    }
}