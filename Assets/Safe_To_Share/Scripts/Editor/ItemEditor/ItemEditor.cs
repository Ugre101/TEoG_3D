using System;
using Safe_To_Share.Scripts.Static;
using UnityEditor;
using UnityEngine;

namespace Items.Editor {
    [CustomEditor(typeof(Item))]
    public sealed class ItemEditor : UnityEditor.Editor {
        const int Rows = 2;
        readonly string[] options = { "Heal", };

        SerializedProperty effectsTree;

        Item myTarget;
        SerializedProperty showEffect;

        void OnEnable() {
            myTarget = (Item)target;

            effectsTree = serializedObject.FindProperty("effectsTree");
        }

        public override void OnInspectorGUI() {
            var length = ItemEffectsTree.PropertyNames.Length;
            var effectIndex = 0;
            var leftOver = length % Rows;
            for (var r = 0; r < Rows; r++) {
                var rowLength = Mathf.FloorToInt((float)length / Rows);
                if (leftOver > 0) {
                    rowLength++;
                    leftOver--;
                }

                EditorGUILayout.BeginHorizontal();
                for (var i = 0; i < rowLength; i++) {
                    if (effectIndex >= ItemEffectsTree.PropertyNames.Length)
                        break;
                    DrawEffectBtn(effectIndex);
                    effectIndex++;
                }

                EditorGUILayout.EndHorizontal();
            }


            if (showEffect != null)
                EditorGUILayout.PropertyField(showEffect);


            base.OnInspectorGUI();
            // Rect rect = EditorGUILayout.BeginVertical("box");
            // EditorGUILayout.LabelField("Summary", EditorStyles.boldLabel);
            // EditorGUILayout.LabelField("Name", myTarget.Title);
            // index = EditorGUILayout.Popup(index, options);
            // EditorGUILayout.EndVertical();
            // serializedObject.Update();
            // EffectEditor.PaintEffectTree(effectsTree, ref showEffect);
            // serializedObject.ApplyModifiedProperties();
            // name = EditorGUILayout.TextField(name);
        }

        void DrawEffectBtn(int effectIndex) {
            string CleanedTypeName(string value) {
                var indexOf = value.IndexOf("Item", StringComparison.Ordinal);
                value = value.Substring(0, indexOf);
                return value;
            }

            var propertyName = ItemEffectsTree.PropertyNames[effectIndex];
            var cleanName = CleanedTypeName(propertyName);
            var orgColor = GUI.backgroundColor;
            var itemEffect = effectsTree.FindPropertyRelative(propertyName);
            GUI.backgroundColor = itemEffect.FindPropertyRelative("active").boolValue ? Color.green : Color.gray;
            if (GUILayout.Button(UgreTools.StringFormatting.AddSpaceAfterCapitalLetter(cleanName)))
                showEffect = effectsTree.FindPropertyRelative(propertyName);
            GUI.backgroundColor = orgColor;
        }
    }
}