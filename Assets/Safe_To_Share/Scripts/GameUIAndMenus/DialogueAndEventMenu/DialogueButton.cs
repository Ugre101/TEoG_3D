using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogue.UI
{
    public class DialogueButton : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI buttonText;
        [SerializeField] Button btn;
        DialogueBaseNode dialogueNode;
        public static event Action<DialogueBaseNode> ChoosesOption;

        public void Setup(DialogueBaseNode dialogueBaseNode)
        {
            dialogueNode = dialogueBaseNode;
            buttonText.text = dialogueBaseNode.PlayerText;
            btn.onClick.AddListener(Click);
        }

        void Click() => ChoosesOption?.Invoke(dialogueNode);

        public void SetupBlocked(DialogueBaseNode childNode)
        {
            buttonText.text = childNode.PlayerText;
            Color faded = buttonText.color;
            faded.a = 0.3f;
            buttonText.color = faded;
            buttonText.fontStyle = FontStyles.Strikethrough;
        }
    }
}