using UnityEngine;

namespace Battle
{
    public class BattleCameraController : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start() => Cursor.lockState = CursorLockMode.None;
    }
}