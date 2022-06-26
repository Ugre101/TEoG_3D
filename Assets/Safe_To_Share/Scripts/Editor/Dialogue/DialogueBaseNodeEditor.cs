using System;
using System.Linq;
using Dialogue;
using Dialogue.DialogueActions;
using UnityEditor;
using UnityEngine;

namespace Editors.Dialogue
{
    [CustomEditor(typeof(DialogueBaseNode))]
    public class DialogueBaseNodeEditor : Editor
    {
        DialogueBaseNode myTarget;
        SerializedProperty actions;

        bool showActions;
        void OnEnable()
        {
            myTarget = (DialogueBaseNode)target;
            actions = serializedObject.FindProperty("actions");
        }

        int selected;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            showActions = EditorGUILayout.Foldout(showActions, "Actions");
            if (showActions)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < actions.arraySize; i++)
                {
                    string typeName = actions.GetArrayElementAtIndex(i).type;
                    int startIndex = typeName.IndexOf("<", StringComparison.Ordinal) + 1;
                    int lenght = typeName.IndexOf(">", StringComparison.Ordinal) - startIndex;
                    typeName = typeName.Substring(startIndex, lenght);
                    EditorGUILayout.PropertyField(actions.GetArrayElementAtIndex(i),new GUIContent(typeName),true);
                }
                EditorGUI.indentLevel--;
            }

            selected = EditorGUILayout.Popup("Action type", selected, actionsTypes);
            if (GUILayout.Button("Add takeHome"))
            {
                myTarget.AddAction(selected);
            }
        }
        public string[] actionsTypes = {"Add To Dorm", "Release Prey", "Add vore temp mods",  };
    }
}