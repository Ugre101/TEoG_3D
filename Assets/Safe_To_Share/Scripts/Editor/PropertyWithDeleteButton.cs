using UnityEditor;
using UnityEngine;

namespace Assets.Editors
{
    public class PropertyWithDeleteButton : PropertyDrawer
    {
        public static float PropertyHeight => 25f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            //   position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            Rect perk = new Rect(position.x, position.y, position.width - 30, PropertyHeight);
            EditorGUI.PropertyField(perk, property, GUIContent.none);

            Rect btn = new Rect(perk.xMax, position.y, 30, PropertyHeight);
            if (GUI.Button(btn, "X"))
            {
                if (property.objectReferenceValue != null)
                    property.DeleteCommand();
                property.DeleteCommand();
            }

            EditorGUI.EndProperty();
        }
    }
}