using CustomClasses.Editor;
using Safe_To_Share.Scripts.QuestStuff;
using UnityEditor;
using UnityEngine;

namespace Safe_To_Share.Scripts.Editor.Quest
{
    public class QuestBuilderWindow : BaseNodeBuilderWindows<QuestStuff.Quest,QuestNode>
    {
        string createType;

        const string BaseOption = "Base";
        static readonly string[] Options = {
            BaseOption
        };

        void OnGUI()
        {
            if (Selected == null)
            {
                EditorGUILayout.LabelField("No selected");
            }
            else
            {
                ProcessEvents();
                ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos, true, true);
                EditorGUILayout.LabelField(Selected.name);

                var canvas = GUILayoutUtility.GetRect(CanvasSize, CanvasSize);
                foreach (QuestNode node in Selected.GetAllNodes())
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
                    AddNewNode();
                }
            }
        }

        [MenuItem("MENUITEM/Quest Builder")]
        static void ShowWindow()
        {
            var window = GetWindow<QuestBuilderWindow>();
            window.titleContent = new GUIContent("TITLE");
            window.Show();
        }

        void AddNewNode()
        {
          
            switch (createType)
            {
                case BaseOption:
                Selected.CreateChildNode<QuestNode>(creatingNode);
                break;
            }
            creatingNode = null;
        }

        int i;
        void DrawNode(QuestNode node)
        {
            GUILayout.BeginArea(node.rect, NodeStyle);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.LabelField(node.GetType().Name, EditorStyles.whiteLabel);
            // EditorGUILayout.LabelField(node.Title);
            EditorGUILayout.LabelField("Add child of type");
            i = EditorGUILayout.Popup(i, Options);
            createType = Options[i];
            // nodeType = UgreTools.IntToEnum(EditorGUILayout.Popup(nodeType.IndexOfEnum(), options), NodeTypes.Basic);
            PrintButtons(node);
            GUILayout.EndArea();
        }
    }
}