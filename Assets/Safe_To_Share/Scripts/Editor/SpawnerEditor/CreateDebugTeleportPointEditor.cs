using Safe_To_Share.Scripts.DebugTools;
using UnityEditor;
using UnityEngine;

namespace Spawner.Editor {
    [CustomEditor(typeof(CreateDebugTeleportPoint))]
    public sealed class CreateDebugTeleportPointEditor : UnityEditor.Editor {
        CreateDebugTeleportPoint myTarget;


        void OnEnable() => myTarget = (CreateDebugTeleportPoint)target;

        void OnSceneGUI() {
            if (!Application.isEditor) return;

            var e = Event.current;


            if (e.control && e.type == EventType.MouseDown) {
                var controlID = GUIUtility.GetControlID(FocusType.Passive);
                GUIUtility.hotControl = controlID;
                var ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

                if (Physics.Raycast(ray, out var hit, float.MaxValue, myTarget.validRaycastTargets))
                    myTarget.AddNewPoint(hit.point);

                e.Use();
            }
        }

        public override void OnInspectorGUI() {
            EditorGUILayout.LabelField("hold control and left click mouse to add point");
            base.OnInspectorGUI();
        }
    }
}