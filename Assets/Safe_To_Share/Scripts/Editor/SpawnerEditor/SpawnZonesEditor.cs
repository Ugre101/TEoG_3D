using Map.Spawner;
using UnityEditor;
using UnityEngine;

namespace Spawner.Editor
{
    [CustomEditor(typeof(SpawnZones))]
    public class SpawnZonesEditor : UnityEditor.Editor
    {
        SpawnZones myTarget;


        void OnEnable() => myTarget = (SpawnZones)target;

        void OnSceneGUI()
        {
            if (!Application.isEditor) return;

            Event e = Event.current;


            if (e.control && e.type == EventType.MouseDown)
            {
                int controlID = GUIUtility.GetControlID(FocusType.Passive);
                GUIUtility.hotControl = controlID;
                Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit)) myTarget.AddNewZone(hit.point);

                e.Use();
            }
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("hold control and left click mouse to spawn spawnzone");
            base.OnInspectorGUI();
        }
    }
}