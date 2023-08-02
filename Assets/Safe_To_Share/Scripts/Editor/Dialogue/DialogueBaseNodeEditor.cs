using System;
using Dialogue;
using UnityEditor;
using UnityEngine;

namespace Editors.Dialogue {
    [CustomEditor(typeof(DialogueBaseNode))]
    public sealed class DialogueBaseNodeEditor : Editor {
        public string[] actionsTypes = { "Add To Dorm", "Release Prey", "Add vore temp mods", "Spot new island", };
        SerializedProperty actions;
        DialogueBaseNode myTarget;

        int selected;

        bool showActions;

        void OnEnable() {
            myTarget = (DialogueBaseNode)target;
            actions = serializedObject.FindProperty("actions");
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            showActions = EditorGUILayout.Foldout(showActions, "Actions");
            if (showActions)
                PrintActions();

            selected = EditorGUILayout.Popup("Action type", selected, actionsTypes);
            if (GUILayout.Button("Add Event")) myTarget.AddAction(selected);
        }

        void PrintActions() {
            EditorGUI.indentLevel++;
            for (var i = 0; i < actions.arraySize; i++) {
                var typeName = actions.GetArrayElementAtIndex(i).type;
                var startIndex = typeName.IndexOf("<", StringComparison.Ordinal) + 1;
                var lenght = typeName.IndexOf(">", StringComparison.Ordinal) - startIndex;
                typeName = typeName.Substring(startIndex, lenght);
                EditorGUILayout.PropertyField(actions.GetArrayElementAtIndex(i), new GUIContent(typeName), true);
            }

            EditorGUI.indentLevel--;
        }
    }
}