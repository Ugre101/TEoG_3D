using Map.Spawner;
using UnityEditor;
using UnityEngine;

namespace Spawner.Editor
{
    [CustomEditor(typeof(SpawnZone))]
    public class SpawnZoneEditor : UnityEditor.Editor
    {
        SpawnZone myTarget;


        void OnEnable() => myTarget = (SpawnZone)target;

        bool addingEnemy;
        void OnSceneGUI()
        {
            if (!Application.isEditor) return;

            Event e = Event.current;


            if (e.shift && e.type == EventType.MouseDown)
            {
                int controlID = GUIUtility.GetControlID(FocusType.Passive);
                GUIUtility.hotControl = controlID;
                Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                if (myTarget.AddSpawnPosition(ray))
                {
                }

                e.Use();
            }
        }

     
        
    }
}