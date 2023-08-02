using Safe_To_Share.Scripts.Map.Spawner;
using UnityEditor;
using UnityEngine;

namespace Spawner.Editor {
    [CustomEditor(typeof(SpawnZone))]
    public sealed class SpawnZoneEditor : UnityEditor.Editor {
        bool addingEnemy;
        SpawnZone myTarget;


        void OnEnable() => myTarget = (SpawnZone)target;

        void OnSceneGUI() {
            if (!Application.isEditor) return;

            var e = Event.current;


            if (e.shift && e.type == EventType.MouseDown) {
                var controlID = GUIUtility.GetControlID(FocusType.Passive);
                GUIUtility.hotControl = controlID;
                var ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                if (myTarget.AddSpawnPosition(ray)) { }

                e.Use();
            }
        }
    }
}