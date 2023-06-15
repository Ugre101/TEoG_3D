using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Static.Editor {
    public static class UgreEditorTools {
        static readonly GUIStyle BoxStyle = new(GUI.skin.box) {
            border = new RectOffset(2, 2, 2, 2),
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
        };

        public static void InspectorButton(Action onClick, string btnText, float height = 30) {
            var btnRect = GUILayoutUtility.GetRect(0, height, GUILayout.ExpandWidth(true));
            if (GUI.Button(btnRect, btnText))
                onClick?.Invoke();
        }

        public static IEnumerable<Object> DropBox(string centerText = "Drop box", float height = 50) {
            var box = GUILayoutUtility.GetRect(0, height, GUILayout.ExpandWidth(true));
            GUI.Box(box, centerText, BoxStyle);
            var ev = Event.current;
            if (!box.Contains(ev.mousePosition)) yield return null;
            if (ev.type != EventType.DragExited) yield return null;
            foreach (var t in DragAndDrop.objectReferences)
                yield return t;
        }
    }
}