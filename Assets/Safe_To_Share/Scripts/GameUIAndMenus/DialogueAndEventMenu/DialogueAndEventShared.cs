using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dialogue;
using Dialogue.UI;
using Safe_to_Share.Scripts.CustomClasses;
using Safe_To_Share.Scripts.Static;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameUIAndMenus.DialogueAndEventMenu
{
    public abstract class DialogueAndEventShared : GameMenu
    {
        protected static float textSpeed = 0.004f;
        static WaitForSecondsRealtime betweenCharsDelay = new(TextSpeed);
        static readonly WaitForSecondsRealtime NewLineDelay = new(0.5f);
        [SerializeField] protected TextMeshProUGUI title;
        [SerializeField] protected TextMeshProUGUI text;
        [SerializeField] protected Transform content;
        [SerializeField] ScrollRect scrollRect;
        [SerializeField] protected Slider slider;
        [SerializeField] protected Button skipBtn;
        [SerializeField] DialogueButton prefab;
        protected BaseDialogue currentDialogue;
        protected DialogueBaseNode currentNode;
        Coroutine printDialogue;

        static float TextSpeed
        {
            get => textSpeed;
            set
            {
                textSpeed = value;
                betweenCharsDelay = new WaitForSecondsRealtime(textSpeed);
            }
        }

        protected virtual void Start()
        {
            DialogueButton.ChoosesOption += HandleOption;
            slider.maxValue = 0.1f;
            slider.minValue = 0.00001f;
            slider.value = textSpeed;
            slider.onValueChanged.AddListener(ChangeTextSpeed);
            skipBtn.onClick.AddListener(Skip);
        }

        protected virtual void OnDestroy() => DialogueButton.ChoosesOption -= HandleOption;

        protected static void ChangeTextSpeed(float arg0) => TextSpeed = arg0;

        protected IEnumerator PrintText(IEnumerable<string> nodeText)
        {
            text.text = string.Empty;

            StringBuilder sb = new();
            foreach (string s in nodeText)
            {
                foreach (char c in s)
                {
                    sb.Append(c);
                    text.text = sb.ToString();
                    scrollRect.verticalNormalizedPosition = 0;
                    scrollRect.verticalScrollbar.value = 0;
                    yield return betweenCharsDelay;
                }

                sb.AppendLine();
                sb.AppendLine();
                yield return NewLineDelay;
            }
        }

        public override bool BlockIfActive() => false;

        protected void ShowNodeText(DialogueBaseNode node)
        {
            title.text = node.Title;
            printDialogue = StartCoroutine(PrintText(node.Text));

            // text.text = node.Text;
        }

        protected void Skip()
        {
            if (printDialogue != null)
                StopCoroutine(printDialogue);
            StringBuilder sb = new();
            foreach (string s in currentNode.Text)
            {
                sb.AppendLine(s);
                sb.AppendLine();
            }

            text.text = sb.ToString();
        }

        protected abstract void HandleOption(DialogueBaseNode obj);

        protected void AddOptionButtons(BaseEditorCanvasNode obj)
        {
            content.KillChildren();
            foreach (DialogueBaseNode childNode in currentDialogue.GetChildNodes(obj).Cast<DialogueBaseNode>())
                if (childNode.ShowNode)
                    Instantiate(prefab, content).Setup(childNode);
                else
                    Instantiate(prefab, content).SetupBlocked(childNode);
        }
    }
}