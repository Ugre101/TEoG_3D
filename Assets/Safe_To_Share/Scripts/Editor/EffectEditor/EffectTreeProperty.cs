using Safe_To_Share.Scripts.Battle.EffectStuff;
using UnityEditor;
using UnityEngine;

namespace EffectStuff.Editor {
    [CustomPropertyDrawer(typeof(EffectsTree))]
    public class EffectTreeProperty : PropertyDrawer {
        bool folded;
        SerializedProperty showEffect;

        public override void OnGUI(Rect position, SerializedProperty property,
                                   GUIContent label) {
            folded = EditorGUILayout.Foldout(folded, "Folded");
            if (!folded)
                return;
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginHorizontal();

            string[] effects = { "dealDamage", "dealWillDamage", "healDamage", };
            foreach (var effect in effects) {
                var tempShowEffect = property.FindPropertyRelative(effect);
                var active = tempShowEffect.FindPropertyRelative("active");
                // GUIStyle btnSkin = new GUIStyle(GUI.skin.button);
                var orgColor = GUI.backgroundColor;
                GUI.backgroundColor = active.boolValue ? Color.green : Color.gray;
                if (GUILayout.Button(effect)) showEffect = tempShowEffect;

                GUI.backgroundColor = orgColor;
            }

            EditorGUILayout.EndHorizontal();
            if (showEffect == null) return;

            EditorGUILayout.PropertyField(showEffect);

            EditorGUI.indentLevel--;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
            base.GetPropertyHeight(property, label);
    }
}