using AvatarStuff;
using AvatarStuff.Holders;
using UnityEngine;

public class TriggerMirrorMenu : MonoBehaviour
{
    [SerializeField] ChangeAvatarDetails mirrorUI;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerHolder mover))
        {
            mirrorUI.Enter(mover);
            mover.PersonEcm2Character.Stop();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            mirrorUI.gameObject.SetActive(false);
    }
}