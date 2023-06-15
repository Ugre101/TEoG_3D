using Map.Spawner;
using UnityEditor;
using UnityEngine;

namespace Spawner.Editor {
    [CustomEditor(typeof(SpawnZones))]
    public sealed class SpawnZonesEditor : UnityEditor.Editor {
        SpawnZones myTarget;


        void OnEnable() => myTarget = (SpawnZones)target;

        void OnSceneGUI() {
            if (!Application.isEditor) return;

            var e = Event.current;


            if (e.control && e.type == EventType.MouseDown) {
                var controlID = GUIUtility.GetControlID(FocusType.Passive);
                GUIUtility.hotControl = controlID;
                var ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

                if (Physics.Raycast(ray, out var hit)) myTarget.AddNewZone(hit.point);

                e.Use();
            }
        }

        public override void OnInspectorGUI() {
            EditorGUILayout.LabelField("hold control and left click mouse to spawn spawnzone");
            base.OnInspectorGUI();
        }
    }
}