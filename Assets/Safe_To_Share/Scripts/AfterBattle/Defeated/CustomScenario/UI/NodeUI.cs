using System;
using System.Collections.Generic;
using System.Linq;
using Character.DefeatScenarios.Custom;
using Safe_To_Share.Scripts.Static;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Safe_To_Share.Scripts.AfterBattle.Defeated.CustomScenario.UI {
    public class NodeUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler {
        static bool binding;
        [SerializeField] TextMeshProUGUI nodeTitle;
        [SerializeField] Slider slider;
        [SerializeField] TextMeshProUGUI resistCost;
        [SerializeField] Button createChild;
        [SerializeField] Button bindToChild;
        [SerializeField] TextMeshProUGUI linkingBtnText;
        [SerializeField] Button deleteNode;
        [SerializeField] TMP_Dropdown childType;
        [SerializeField] Transform linkContainer;
        [SerializeField] GameObject childLink;
        [SerializeField] List<PosAndRot> linkPoses;
        bool bindingMe;
        CustomNodeTypes createChildOfType = CustomNodeTypes.EssenceDrain;
        CustomLoseScenarioNode node;
        CustomLoseScenario scenario;

        void OnDestroy() {
            CancelBinding -= ResetBindingText;
            StartLinking -= ChangeLinkingText;
            FinishLinking -= DoneLinking;
            NodeMoved -= DidMyChildMove;
        }

        public void OnBeginDrag(PointerEventData eventData) { }

        public void OnDrag(PointerEventData eventData) => transform.position = eventData.position;

        public void OnEndDrag(PointerEventData eventData) {
            node.canvasPos = transform.localPosition;
            ShowLinks();
            NodeMoved?.Invoke(node.id);
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (node != null)
                ShowMe?.Invoke(node);
        }

        void CreateChild() {
            node.childNodesIds.Add(scenario.CreateNewNode(createChildOfType, node.canvasPos + new Vector2(250, 0)));
            ShowLinks();
        }

        void ChangeChildType(int arg0) =>
            createChildOfType =
                UgreTools.IntToEnum(arg0, CustomNodeTypes.EssenceDrain, CustomNodeTypes.Intro);

        protected virtual void ResistChange(float arg0) => resistCost.text = Mathf.RoundToInt(arg0).ToString();

        public void SetupNode(CustomLoseScenario scenario, CustomLoseScenarioNode node) {
            this.scenario = scenario;
            this.node = node;

            nodeTitle.text = GetTitle();

            slider.onValueChanged.AddListener(ResistChange);
            childType.SetupTmpDropDown(CustomNodeTypes.EssenceDrain, ChangeChildType, CustomNodeTypes.Intro);
            createChild.onClick.AddListener(CreateChild);
            if (node is CustomIntroNode)
                deleteNode.gameObject.SetActive(false);
            else
                deleteNode.onClick.AddListener(DeleteThisNode);
            bindToChild.onClick.AddListener(ClickLinkingNodesAction);

            StartLinking += ChangeLinkingText;
            FinishLinking += DoneLinking;
            CancelBinding += ResetBindingText;
            NodeMoved += DidMyChildMove;

            string GetTitle() =>
                node switch {
                    CustomIntroNode introNode => nodeTitle.text = "Intro",
                    CustomBodyNode bodyNode => nodeTitle.text = "Body",
                    CustomDrainNode drainNode => nodeTitle.text = "Essence",
                    CustomVoreNode voreNode => nodeTitle.text = "Vore",
                    _ => string.Empty,
                };
        }

        void DidMyChildMove(string obj) {
            if (node.childNodesIds.Contains(obj))
                ShowLinks();
        }

        public void ShowLinks() {
            linkContainer.KillChildren();
            if (node.childNodesIds.Count == 0) return;
            foreach (var childId in node.childNodesIds) PrintLink(childId);
        }

        void PrintLink(string childId) {
            if (!scenario.ChildNodesDict.TryGetValue(childId, out var targetNode))
                return;
            var obj = Instantiate(childLink, linkContainer);
            Vector3 dir = targetNode.canvasPos - node.canvasPos;
            var angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            foreach (var linkPos in linkPoses.Where(linkPos => angle >= linkPos.min && angle < linkPos.max)) {
                obj.transform.localPosition = linkPos.pos;
                obj.transform.eulerAngles = new Vector3(0, 0, linkPos.zAngle);
                break;
            }
        }

        void ResetBindingText() => linkingBtnText.text = "Start Linking";

        void DoneLinking(string obj) {
            ResetBindingText();
            if (!bindingMe)
                return;
            bindingMe = false;
            if (scenario.NotLooping(node, obj))
                node.childNodesIds.Add(obj);
        }

        void ChangeLinkingText() {
            if (bindingMe)
                linkingBtnText.text = "Cancel";
            else
                linkingBtnText.text = "Link To";
        }

        static event Action<string> NodeMoved;
        static event Action StartLinking;
        static event Action<string> FinishLinking;
        static event Action CancelBinding;

        void ClickLinkingNodesAction() {
            if (binding) {
                binding = false;
                CompleteBinding();
            } else {
                binding = true;
                bindingMe = true;
                StartLinking?.Invoke();
            }
        }

        void CompleteBinding() {
            if (bindingMe) {
                bindingMe = false;
                ResetBindingText();
                CancelBinding?.Invoke();
            } else {
                ResetBindingText();
                FinishLinking?.Invoke(node.id);
            }
        }

        void DeleteThisNode() {
            OnDestroy();
            scenario.RemoveNode(node);
            Destroy(gameObject);
        }

        public event Action<CustomLoseScenarioNode> ShowMe;

        [Serializable]
        struct PosAndRot {
            public float min, max;
            public Vector2 pos;
            public float zAngle;
        }
    }
}