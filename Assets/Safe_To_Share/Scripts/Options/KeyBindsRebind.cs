using UnityEngine;
using UnityEngine.InputSystem;

namespace Options
{
    public class KeyBindsRebind : MonoBehaviour
    {
        [SerializeField] PlayerInput playerInput;
        [SerializeField] RebindButton rebindButton;
        [SerializeField] Transform content;

        void Start()
        {
            content.KillChildren();
            foreach (InputAction inputBinding in playerInput.currentActionMap.actions)
                Instantiate(rebindButton, content).Setup(inputBinding);
        }
    }
}