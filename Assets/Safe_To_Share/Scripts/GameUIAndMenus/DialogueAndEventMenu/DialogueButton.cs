using System;
using Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.GameUIAndMenus.DialogueAndEventMenu {
    public sealed class DialogueButton : MonoBehaviour {
        [SerializeField] TextMeshProUGUI buttonText;
        [SerializeField] Button btn;
        [SerializeField] InputAction inputAction;
        [SerializeField] TextMeshProUGUI hotKeyText;
        DialogueBaseNode dialogueNode;

        void OnDestroy() {
            inputAction.Disable();
        }

        public static event Action<DialogueBaseNode> ChoosesOption;

        public void Setup(DialogueBaseNode dialogueBaseNode, int i) {
            dialogueNode = dialogueBaseNode;
            buttonText.text = dialogueBaseNode.PlayerText;
            btn.onClick.AddListener(Click);
            inputAction.AddBinding($"<Keyboard>/{i + 1}");
            hotKeyText.text = inputAction.GetBindingDisplayString();
            inputAction.performed += _ => Click();
            inputAction.Enable();
        }

        void Click() => ChoosesOption?.Invoke(dialogueNode);

        public void SetupBlocked(DialogueBaseNode childNode, int i) {
            buttonText.text = childNode.PlayerText;
            var faded = buttonText.color;
            faded.a = 0.3f;
            buttonText.color = faded;
            buttonText.fontStyle = FontStyles.Strikethrough;
        }
    }
}