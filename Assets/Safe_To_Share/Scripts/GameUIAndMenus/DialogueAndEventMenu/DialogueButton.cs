using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Dialogue.UI
{
    public class DialogueButton : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI buttonText;
        [SerializeField] Button btn;
        [SerializeField] InputAction inputAction;
        [SerializeField] TextMeshProUGUI hotKeyText;
        DialogueBaseNode dialogueNode;
        public static event Action<DialogueBaseNode> ChoosesOption;

        public void Setup(DialogueBaseNode dialogueBaseNode, int i)
        {
            dialogueNode = dialogueBaseNode;
            buttonText.text = dialogueBaseNode.PlayerText;
            btn.onClick.AddListener(Click);
            inputAction.AddBinding($"<Keyboard>/{i + 1}");
            hotKeyText.text = inputAction.GetBindingDisplayString();
            inputAction.performed += _ => Click();
            inputAction.Enable();
        }

        void OnDestroy()
        {
            inputAction.Disable();
        }

        void Click() => ChoosesOption?.Invoke(dialogueNode);

        public void SetupBlocked(DialogueBaseNode childNode, int i)
        {
            buttonText.text = childNode.PlayerText;
            Color faded = buttonText.color;
            faded.a = 0.3f;
            buttonText.color = faded;
            buttonText.fontStyle = FontStyles.Strikethrough;
        }
    }
}