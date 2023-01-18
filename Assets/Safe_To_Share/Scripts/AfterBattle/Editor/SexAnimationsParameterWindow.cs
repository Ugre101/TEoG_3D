using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle.Editor
{
    public class SexAnimationsParameterWindow : EditorWindow
    {
        AnimatorController selected;

        void OnEnable()
        {
            Selection.selectionChanged += SelectionChanged;
        }

        void OnDisable()
        {
            Selection.selectionChanged -= SelectionChanged;
        }

        [MenuItem("MENUITEM/Sex animations parameters")]
        private static void ShowWindow()
        {
            var window = GetWindow<SexAnimationsParameterWindow>();
            window.titleContent = new GUIContent("Parameters");
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("Name and Hash", EditorStyles.boldLabel);
            if (selected == null)
                return;
            foreach (var para in selected.parameters)
            {
                EditorGUILayout.BeginHorizontal("Box");
                EditorGUILayout.LabelField(para.name );
                EditorGUILayout.LabelField(para.nameHash.ToString());
                if (GUILayout.Button("Copy"))
                {
                    EditorGUIUtility.systemCopyBuffer = para.nameHash.ToString();
                }
                EditorGUILayout.EndHorizontal();
            }

        }

        void SelectionChanged()
        {
            var newObjet = Selection.activeObject;
            if (newObjet is AnimatorController controller)
            {
                selected = controller;
                Repaint();
            }
        }
    }
}