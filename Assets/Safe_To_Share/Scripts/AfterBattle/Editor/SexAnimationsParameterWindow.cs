using UnityEditor;
using UnityEngine;

namespace Safe_To_Share.Scripts.AfterBattle.Editor
{
    public class SexAnimationsParameterWindow : EditorWindow
    {
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
            if (AnimatorControllerParameterGrabberSObj.instance == null)
                return;
            foreach (var para in AnimatorControllerParameterGrabberSObj.instance.Parameters)
            {
                EditorGUILayout.BeginHorizontal();
                GUI.enabled = false;
                EditorGUILayout.TextField("Name", para.Name );
                EditorGUILayout.IntField("Hash", para.Hash);
                GUI.enabled = true;
                if (GUILayout.Button("Copy"))
                {
                    EditorGUIUtility.systemCopyBuffer = para.Hash.ToString();
                }
                EditorGUILayout.EndHorizontal();
            }

        }
    }
}