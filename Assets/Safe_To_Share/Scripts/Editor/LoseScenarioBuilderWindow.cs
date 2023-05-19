using System;
using System.Linq;
using Character.DefeatScenarios.Nodes;
using CustomClasses.Editor;
using Safe_To_Share.Scripts.Static;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Defeated.Editor
{
    public sealed class LoseScenarioBuilderWindow : BaseNodeBuilderWindows<LoseScenario,LoseScenarioNode>
    {
        [NonSerialized] readonly string[] options =
        {
            NodeTypes.Basic.ToString(),
            NodeTypes.Drain.ToString(),
            NodeTypes.Body.ToString(),
            NodeTypes.Vore.ToString(),
        };

        NodeTypes createOfType = NodeTypes.Body;

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
                foreach (LoseScenarioNode node in Selected.GetAllNodes())
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
                {
                    switch (createOfType)
                    {
                        case NodeTypes.Basic:
                            Selected.CreateChildNode<LoseScenarioNode>(creatingNode);
                            break;
                        case NodeTypes.Drain:
                            Selected.CreateChildNode<LoseScenarioDrainEssenceNode>(creatingNode);
                            break;
                        case NodeTypes.Body:
                            Selected.CreateChildNode<LoseScenarioNodeBodyMorph>(creatingNode);
                            break;
                        case NodeTypes.Vore:
                            Selected.CreateChildNode<LoseScenarioNodeTriesToVore>(creatingNode);
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

        void DrawNode(LoseScenarioNode node)
        {
            GUILayout.BeginArea(node.rect, NodeStyle);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.LabelField(node.GetType().Name.Remove(0, 12), EditorStyles.whiteLabel);
            //EditorGUILayout.LabelField(node.Title);
            //EditorGUILayout.LabelField("Add child of type");
            createOfType = UgreTools.IntToEnum(EditorGUILayout.Popup(createOfType.IndexOfEnum(), options),
                NodeTypes.Drain);
            PrintButtons(node);

            GUILayout.EndArea();
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