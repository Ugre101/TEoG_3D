using AvatarStuff.Holders;
using Character.PlayerStuff;
using UnityEngine;

namespace AvatarStuff.UI
{
    public class EnterBodyMorphPanel : MonoBehaviour
    {
        [SerializeField] BodyMorphPanel panel;
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && other.TryGetComponent(out PlayerHolder mover))
            {
                panel.Enter(mover);
                mover.PersonEcm2Character.Stop();
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) 
                return;
            panel.gameObject.SetActive(false);
        }
    }
}