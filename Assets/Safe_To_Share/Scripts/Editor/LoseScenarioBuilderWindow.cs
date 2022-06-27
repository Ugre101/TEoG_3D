using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Character.DefeatScenarios.Nodes;
using CustomClasses.Editor;
using Safe_To_Share.Scripts.Static;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Defeated.Editor
{
    public class LoseScenarioBuilderWindow : BaseNodeBuilderWindows
    {
        LoseScenario selected;

        NodeTypes createOfType = NodeTypes.Body;
        [NonSerialized]
        readonly string[] options =
        {
            NodeTypes.Basic.ToString(),
            NodeTypes.Drain.ToString(),
            NodeTypes.Body.ToString(),
            NodeTypes.Vore.ToString(),
        };
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
                foreach (LoseScenarioNode node in selected.GetAllNodes().Cast<LoseScenarioNode>())
                {
                    DrawNode(node);
                    DrawNodeConnection(selected,node);
                }

                EditorGUILayout.EndScrollView();

                if (deletingNode != null)
                {
                    selected.DeleteChildNode(deletingNode);
                    deletingNode = null;
                }
                else if (creatingNode != null)
                {
                    switch (createOfType)
                    {
                        case NodeTypes.Basic:
                            selected.CreateChildNode<LoseScenarioNode>(creatingNode);
                            break;
                        case NodeTypes.Drain:
                            selected.CreateChildNode<LoseScenarioDrainEssenceNode>(creatingNode);
                            break;
                        case NodeTypes.Body:
                            selected.CreateChildNode<LoseScenarioNodeBodyMorph>(creatingNode);
                            break;
                        case NodeTypes.Vore:
                            selected.CreateChildNode<LoseScenarioNodeTriesToVore>(creatingNode);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    creatingNode = null;
                }
            }
        }

      
        [MenuItem("Defeat/Lose Scenario Builder")]
        static void ShowWindow()
        {
            var window = GetWindow<LoseScenarioBuilderWindow>();
            window.titleContent = new GUIContent("Lose Scenario");
            window.Show();
        }

        protected override void SelectionChanged()
        {
            var newObjet = Selection.activeObject;
            if (newObjet is LoseScenario loseScenario)
            {
                selected = loseScenario;
                Repaint();
            }
        }

        void DrawNode(LoseScenarioNode node)
        {
            GUILayout.BeginArea(node.rect, NodeStyle);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.LabelField(node.GetType().Name.Remove(0,12), EditorStyles.whiteLabel);
            //EditorGUILayout.LabelField(node.Title);
            //EditorGUILayout.LabelField("Add child of type");
            createOfType = UgreTools.IntToEnum(EditorGUILayout.Popup(createOfType.IndexOfEnum(),options),NodeTypes.Drain);
            PrintButtons(node);

            GUILayout.EndArea();
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

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            Object obj = EditorUtility.InstanceIDToObject(instanceID);
            if (obj is LoseScenario loseScenario)
            {
                ShowWindow();
                return true;
            }

            return false;
        }

        enum NodeTypes
        {
            Basic,
            Drain,
            Body,
            Vore,
        }
    }
}