using Character.VoreStuff;
using Safe_To_Share.Scripts.Static;
using Static;
using UnityEditor;
using UnityEngine;

namespace EditorVore
{
    public class VoredCharactersWindow : EditorWindow
    {
        [MenuItem("MENUITEM/Vored Windows")]
        private static void ShowWindow()
        {
            var window = GetWindow<VoredCharactersWindow>();
            window.titleContent = new GUIContent("Vored");
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("Preys", EditorStyles.boldLabel);
            foreach (Prey prey in VoredCharacters.PreyDict.Values)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.TextField("Prey ID",prey.Identity.ID.ToString());
                EditorGUILayout.TextField("Prey Name",prey.Identity.FullName);
                EditorGUILayout.TextField("Weight",prey.Body.Weight.ConvertKg(true));
                EditorGUILayout.TextField("Alt Digest",$"{prey.AltProgress}");
                if (!string.IsNullOrEmpty(prey.SpecialDigestion))
                    EditorGUILayout.TextField("Special Mode",$"{prey.SpecialDigestion}");
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}