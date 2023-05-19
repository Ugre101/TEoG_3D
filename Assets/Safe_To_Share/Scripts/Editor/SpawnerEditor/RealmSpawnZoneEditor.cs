using Map.Spawner;
using Safe_To_Share.Scripts.Map.Sub_Realm;
using UnityEditor;
using UnityEngine;

namespace Spawner.Editor
{
    [CustomEditor(typeof(RealmEnemyZone))]
    public sealed class RealmSpawnZoneEditor : UnityEditor.Editor
    {
        RealmEnemyZone myTarget;
        bool addingEnemy = true;


        void OnEnable() => myTarget = (RealmEnemyZone)target;

        void OnSceneGUI()
        {
            if (!Application.isEditor) return;

            Event e = Event.current;


            if (e.shift && e.type == EventType.MouseDown) 
                TryAddSpawnPosition(e);
        }

        void TryAddSpawnPosition(Event e)
        {
            int controlID = GUIUtility.GetControlID(FocusType.Passive);
            GUIUtility.hotControl = controlID;
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (addingEnemy)
            {
                if (myTarget.AddSpawnPosition(ray))
                    serializedObject.Update();
            }
            else
            {
                if (myTarget.AddBossPosition(ray))
                    serializedObject.Update();
            }

            e.Use();
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginHorizontal("Box");
            if (GUILayout.Button("Add Enemy Spot", BtnStyle(addingEnemy)))
                addingEnemy = true;
            else if (GUILayout.Button("Add Boss Spot",BtnStyle(!addingEnemy))) 
                addingEnemy = false;
            EditorGUILayout.EndHorizontal();
            //  EditorGUILayout.LabelField("hold control and left click mouse to spawn spawnzone");
            base.OnInspectorGUI();
        }

        static GUIStyle BtnStyle(bool green) => new(GUI.skin.button)
        {
            fontSize = 20,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            normal = {textColor = green ? Color.green : Color.red},
            hover = {textColor = Color.white},
            active = {textColor = Color.white},
            focused = {textColor = Color.white},
            onNormal = {textColor = Color.white},
            onHover = {textColor = Color.white},
            onActive = {textColor = Color.white},
            onFocused = {textColor = Color.white},
        };
    }
}