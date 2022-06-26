using Character.DefeatScenarios.Custom;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.AfterBattle.Defeated.CustomScenario.UI
{
    public class SelectedNode : MonoBehaviour
    {
        public static bool hasSelectedNode;
        [SerializeField] Button introText, resistText, submitText;
        [SerializeField] Image introBtnImage, resistBtnImage, submitBtnImage;
        [SerializeField] TMP_InputField inputField;
        [SerializeField] Transform optionsContainer;
        [SerializeField] BodyNodeOptions bodyNodeOptions;
        [SerializeField] DrainNodeOptions drainNodeOptions;
        [SerializeField] VoreNodeOptions voreNodeOptions;
        CustomLoseScenarioNode selectedNode;
        textType writeType = textType.Intro;

        void Start()
        {
            introText.onClick.AddListener(WriteIntroText);
            resistText.onClick.AddListener(WriteResistText);
            submitText.onClick.AddListener(WriteSubmitText);
            inputField.onValueChanged.AddListener(WriteText);
        }


        void WriteText(string arg0)
        {
            if (!hasSelectedNode)
                return;
            switch (writeType)
            {
                case textType.Intro:
                    selectedNode.introText = arg0;
                    break;
                case textType.Resist:
                    selectedNode.resistText = arg0;
                    break;
                case textType.Submit:
                    selectedNode.giveInText = arg0;
                    break;
            }
        }

        void WriteSubmitText()
        {
            writeType = textType.Submit;
            introBtnImage.color = Color.white;
            resistBtnImage.color = Color.white;
            submitBtnImage.color = Color.green;
            inputField.text = selectedNode.giveInText;
        }

        void WriteResistText()
        {
            writeType = textType.Resist;
            introBtnImage.color = Color.white;
            resistBtnImage.color = Color.green;
            submitBtnImage.color = Color.white;
            inputField.text = selectedNode.resistText;
        }

        void WriteIntroText()
        {
            writeType = textType.Intro;
            introBtnImage.color = Color.green;
            resistBtnImage.color = Color.white;
            submitBtnImage.color = Color.white;
            inputField.text = selectedNode.introText;
        }

        public void Setup(CustomLoseScenarioNode node)
        {
            if (node == selectedNode)
                return;
            hasSelectedNode = true;
            selectedNode = node;
            WriteIntroText();
            switch (node)
            {
                case CustomIntroNode introNode:
                    optionsContainer.SleepChildren();
                    break;
                case CustomDrainNode drainNode:
                    optionsContainer.SleepChildren(drainNodeOptions.gameObject);
                    drainNodeOptions.Setup(drainNode);
                    break;
                case CustomVoreNode voreNode:
                    optionsContainer.SleepChildren(voreNodeOptions.gameObject);
                    voreNodeOptions.Setup(voreNode);
                    break;
                case CustomBodyNode bodyNode:
                    optionsContainer.SleepChildren(bodyNodeOptions.gameObject);
                    bodyNodeOptions.Setup(bodyNode);
                    break;
            }
        }

        enum textType
        {
            Intro,
            Resist,
            Submit,
        }
    }
}