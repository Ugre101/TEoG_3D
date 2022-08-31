using System;
using CustomClasses.Editor;
using Safe_To_Share.Scripts.Static;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Dialogue.Editor
{
    public class DialogueBuilderWindow : BaseNodeBuilderWindows
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
        BaseDialogue selected;

        void OnGUI()
        {
            if (selected == null)
                EditorGUILayout.LabelField("No selected");
            else
            {
                ProcessEvents();
                ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos, true, true);
                EditorGUILayout.LabelField(selected.name);

                Rect canvas = GUILayoutUtility.GetRect(CanvasSize, CanvasSize);
                foreach (DialogueBaseNode node in selected.GetAllNodes())
                {
                    DrawNode(node);
                    DrawNodeConnection(selected, node);
                }

                EditorGUILayout.EndScrollView();

                if (deletingNode != null)
                {
                    selected.DeleteChildNode(deletingNode);
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
                    selected.CreateNewNode<DialogueBaseNode>(creatingNode);
                    break;
                case NodeTypes.Close:
                    selected.CreateNewNode<CloseDialogue>(creatingNode);
                    break;
                case NodeTypes.Quest:
                    selected.CreateNewNode<DialogueQuestNode>(creatingNode);
                    break;
                case NodeTypes.PreBattle:
                    selected.CreateNewNode<PreBattleDialogue>(creatingNode);
                    break;
                case NodeTypes.Service:
                    selected.CreateNewNode<ServiceMenuDialogueNode>(creatingNode);
                    break;
                case NodeTypes.Shop:
                    selected.CreateNewNode<OpenShopDialogueNode>(creatingNode);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            creatingNode = null;
        }

        protected override void SelectionChanged()
        {
            var newObjet = Selection.activeObject;
            if (newObjet is BaseDialogue dialogue)
            {
                selected = dialogue;
                Repaint();
            }
        }

        [MenuItem("Dialogue/Builder")]
        static void ShowWindow()
        {
            var window = GetWindow<DialogueBuilderWindow>();
            window.titleContent = new GUIContent("Dialogue Builder");
            window.Show();
        }

        void ProcessEvents()
        {
            switch (Event.current.type)
            {
                case EventType.MouseDown when draggedNode == null:
                {
                    draggedNode = selected.GetNodeAtPoint(Event.current.mousePosition + ScrollPos);
                    if (draggedNode != null)
                    {
                        DraggingOffset = draggedNode.rect.position - Event.current.mousePosition;
                        Selection.activeObject = draggedNode;
                    }
                    else
                    {
                        DraggingCanvas = true;
                        DraggingCanvasOffset = Event.current.mousePosition + ScrollPos;
                        Selection.activeObject = selected;
                    }

                    break;
                }
                case EventType.MouseDrag when draggedNode != null:
                    Undo.RecordObject(selected, "Move node");
                    draggedNode.rect.position = Event.current.mousePosition + DraggingOffset;
                    GUI.changed = true;
                    break;
                case EventType.MouseDrag when DraggingCanvas:
                    ScrollPos = DraggingCanvasOffset - Event.current.mousePosition;
                    GUI.changed = true;
                    break;
                case EventType.MouseUp when draggedNode != null:
                    draggedNode = null;
                    break;
                case EventType.MouseUp when DraggingCanvas:
                    DraggingCanvas = false;
                    break;
            }
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