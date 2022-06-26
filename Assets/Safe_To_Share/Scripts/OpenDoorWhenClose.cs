using UnityEngine;

namespace Safe_To_Share.Scripts
{
    public class OpenDoorWhenClose : MonoBehaviour
    {
        [SerializeField] Animation openDoor;

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                OpenDoor();
        }

        void OpenDoor()
        {
            openDoor.Play();
            Destroy(gameObject);
        }
    }
}