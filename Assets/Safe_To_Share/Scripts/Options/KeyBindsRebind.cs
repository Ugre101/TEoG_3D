using Safe_To_Share.Scripts.Static;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Safe_To_Share.Scripts.Options
{
    public sealed class KeyBindsRebind : MonoBehaviour
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