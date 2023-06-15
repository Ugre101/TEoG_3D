using Character.VoreStuff;
using Safe_To_Share.Scripts.Static;
using UnityEditor;
using UnityEngine;

namespace EditorVore {
    public sealed class VoredCharactersWindow : EditorWindow {
        void OnGUI() {
            GUILayout.Label("Preys", EditorStyles.boldLabel);
            foreach (var prey in VoredCharacters.PreyDict.Values) {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.TextField("Prey ID", prey.Identity.ID.ToString());
                EditorGUILayout.TextField("Prey Name", prey.Identity.FullName);
                EditorGUILayout.TextField("Weight", prey.Body.Weight.ConvertKg());
                EditorGUILayout.TextField("Alt Digest", $"{prey.AltProgress}");
                if (!string.IsNullOrEmpty(prey.SpecialDigestion))
                    EditorGUILayout.TextField("Special Mode", $"{prey.SpecialDigestion}");
                EditorGUILayout.EndHorizontal();
            }
        }

        [MenuItem("MENUITEM/Vored Windows")]
        static void ShowWindow() {
            var window = GetWindow<VoredCharactersWindow>();
            window.titleContent = new GUIContent("Vored");
            window.Show();
        }
    }
}