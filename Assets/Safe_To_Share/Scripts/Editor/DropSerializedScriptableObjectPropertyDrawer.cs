using CustomClasses;
using UnityEditor;
using UnityEngine;

namespace Editorasd {
    [CustomPropertyDrawer(typeof(DropSerializableObject<>))]
    public class DropSerializedScriptableObjectPropertyDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            var amountRect = new Rect(position.x, position.y, position.width, position.height * (2f / 3f));
            DropAreaGUI(amountRect, property);
            var guidRect = new Rect(position.x, position.y + amountRect.height, position.width,
                position.height * (1f / 3f));
            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.TextField(guidRect, property.FindPropertyRelative("guid").stringValue);
            EditorGUI.EndDisabledGroup();
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
            base.GetPropertyHeight(property, label) * 3f;

        public void DropAreaGUI(Rect rect, SerializedProperty property) {
            var evt = Event.current;
            GUI.Box(rect, "Drop scripableObject with guid");

            switch (evt.type) {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!rect.Contains(evt.mousePosition))
                        return;

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (evt.type == EventType.DragPerform) {
                        DragAndDrop.AcceptDrag();

                        foreach (var dragged_object in DragAndDrop.objectReferences)
                            if (dragged_object is SerializableScriptableObject serilized)
                                property.FindPropertyRelative("guid").stringValue = serilized.Guid;
                    }

                    break;
            }
        }
    }
}