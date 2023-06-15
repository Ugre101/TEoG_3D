using System.Linq;
using Safe_To_Share.Scripts.Static;
using UnityEditor;
using UnityEngine;

namespace EffectStuff.Editor {
    public static class EffectEditor {
        public static void PaintEffectTree(SerializedProperty abilityTree, ref SerializedProperty showEffect) {
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginHorizontal();
            string[] effects = { "dealDamage", "dealWillDamage", "healDamage", "shrinkBody", "growBody", };
            foreach (var effect in effects) {
                var tempShowEffect = abilityTree.FindPropertyRelative(effect);
                var active = tempShowEffect.FindPropertyRelative("active");
                // GUIStyle btnSkin = new GUIStyle(GUI.skin.button);
                var orgColor = GUI.backgroundColor;
                GUI.backgroundColor = active.boolValue ? Color.green : Color.gray;
                var betterTitle = effect.First().ToString().ToUpper() + effect.Substring(1);
                betterTitle = UgreTools.StringFormatting.AddSpaceAfterCapitalLetter(betterTitle);
                if (GUILayout.Button(betterTitle)) showEffect = tempShowEffect;

                GUI.backgroundColor = orgColor;
            }

            EditorGUILayout.EndHorizontal();
            if (showEffect == null) return;
            EditorGUILayout.PropertyField(showEffect);
            // DrawEffect(showEffect);
            EditorGUI.indentLevel--;
        }

        static void DrawEffect(SerializedProperty effect) {
            EditorGUILayout.BeginVertical("box");
            var isActive = effect.FindPropertyRelative("active");
            var value = effect.FindPropertyRelative("value");
            var rngValue = effect.FindPropertyRelative("rngValue");
            var affectedByStats = effect.FindPropertyRelative("affectedByStats");
            if (GUILayout.Button(isActive.boolValue ? "Disable effect" : "Enable effect"))
                isActive.boolValue = !isActive.boolValue;
            value.intValue = EditorGUILayout.IntSlider("Value", value.intValue, 0, 999);
            EditorGUILayout.PropertyField(rngValue);
            EditorGUILayout.PropertyField(affectedByStats, true);
            EditorGUILayout.PropertyField(effect, new GUIContent("Base editor"));
            EditorGUILayout.EndVertical();
        }
    }
}