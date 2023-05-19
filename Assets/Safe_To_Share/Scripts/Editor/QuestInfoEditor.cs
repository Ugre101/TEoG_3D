using QuestStuff;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    [CustomEditor(typeof(QuestInfo))]
    public sealed class QuestInfoEditor : Editor
    {
        SerializedProperty id;
        QuestInfo myTarget;
        string tempID = string.Empty;

        void OnEnable()
        {
            myTarget = (QuestInfo)target;
            id = serializedObject.FindProperty("questId");
        }

        void SetID(string value) => id.stringValue = value;

        public override void OnInspectorGUI()
        {
            Rect rect = EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Summary", EditorStyles.boldLabel);
            EditorGUILayout.EndVertical();
            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();

            // name = EditorGUILayout.TextField(name);
            base.OnInspectorGUI();
        }
    }
}