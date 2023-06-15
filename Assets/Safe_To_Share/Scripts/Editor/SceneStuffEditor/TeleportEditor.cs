using System.IO;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace SceneStuff.Editor {
    [CustomEditor(typeof(SceneChangeTeleport))]
    public sealed class TeleportEditor : UnityEditor.Editor {
        int index;

        SceneChangeTeleport myTarget;
        SerializedProperty sceneTarget;

        void OnEnable() {
            myTarget = (SceneChangeTeleport)target;
            sceneTarget = serializedObject.FindProperty("sceneToLoad");
        }

        public override void OnInspectorGUI() {
            EditorGUI.BeginChangeCheck();
            index = EditorGUILayout.Popup(index, ScenesToList());
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(myTarget, "Changed target map");
                serializedObject.Update();
                sceneTarget.stringValue = ScenesToList()[index];
                serializedObject.ApplyModifiedProperties();
            }

            base.OnInspectorGUI();
        }

        static string[] ScenesToList() {
            var toReturn = new string[SceneManager.sceneCountInBuildSettings];
            for (var i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
                toReturn[i] = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
            return toReturn;
        }
    }
}