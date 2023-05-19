using System;
using CustomClasses.Editor;
using Safe_To_Share.Scripts.Static;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Dialogue.Editor
{
    public sealed class DialogueBuilderWindow : BaseNodeBuilderWindows<BaseDialogue,DialogueBaseNode>
    {
        [NonSerialized] readonly string[] options =
        {
            NodeTypes.Basic.ToString(),
            NodeTypes.Close.ToString(),
            NodeTypes.Quest.ToString(),
            NodeTypes.PreBattle.ToString(),
            NodeTypes.Service.ToString(),
            NodeTypes.Shop.ToString(),
        };

        [NonSerialized] NodeTypes nodeType;

        void OnGUI()
        {
            if (Selected == null)
                EditorGUILayout.LabelField("No selected");
            else
            {
                ProcessEvents();
                ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos, true, true);
                EditorGUILayout.LabelField(Selected.name);

                Rect canvas = GUILayoutUtility.GetRect(CanvasSize, CanvasSize);
                foreach (DialogueBaseNode node in Selected.GetAllNodes())
                {
                    DrawNode(node);
                    DrawNodeConnection(Selected, node);
                }

                EditorGUILayout.EndScrollView();

                if (deletingNode != null)
                {
                    Selected.DeleteChildNode(deletingNode);
                    deletingNode = null;
                }
                else if (creatingNode != null)
                    AddNewNode();
            }
        }

        void AddNewNode()
        {
            switch (nodeType)
            {
                case NodeTypes.Basic:
                    Selected.CreateChildNode<DialogueBaseNode>(creatingNode);
                    break;
                case NodeTypes.Close:
                    Selected.CreateChildNode<CloseDialogue>(creatingNode);
                    break;
                case NodeTypes.Quest:
                    Selected.CreateChildNode<DialogueQuestNode>(creatingNode);
                    break;
                case NodeTypes.PreBattle:
                    Selected.CreateChildNode<PreBattleDialogue>(creatingNode);
                    break;
                case NodeTypes.Service:
                    Selected.CreateChildNode<ServiceMenuDialogueNode>(creatingNode);
                    break;
                case NodeTypes.Shop:
                    Selected.CreateChildNode<OpenShopDialogueNode>(creatingNode);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            creatingNode = null;
        }

        [MenuItem("Dialogue/Builder")]
        static void ShowWindow()
        {
            var window = GetWindow<DialogueBuilderWindow>();
            window.titleContent = new GUIContent("Dialogue Builder");
            window.Show();
        }

        void DrawNode(DialogueBaseNode node)
        {
            GUILayout.BeginArea(node.rect, NodeStyle);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.LabelField(node.GetType().Name, EditorStyles.whiteLabel);
            EditorGUILayout.LabelField(node.Title);
            EditorGUILayout.LabelField("Add child of type");
            nodeType = UgreTools.IntToEnum(EditorGUILayout.Popup(nodeType.IndexOfEnum(), options), NodeTypes.Basic);
            PrintButtons(node);

            GUILayout.EndArea();
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            Object obj = EditorUtility.InstanceIDToObject(instanceID);
            if (obj is BaseDialogue dialogue)
            {
                ShowWindow();
                return true;
            }

            return false;
        }

        enum NodeTypes
        {
            Basic,
            Close,
            Quest,
            PreBattle,
            Service,
            Shop,
        }
    }
}