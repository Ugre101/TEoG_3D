using UnityEngine;

namespace Safe_To_Share.Scripts
{
    public class OpenCheatMenu : MonoBehaviour
    {
        [SerializeField] GameObject gameCanvas;

        void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;
            gameCanvas.SetActive(true);
        }
    }
}