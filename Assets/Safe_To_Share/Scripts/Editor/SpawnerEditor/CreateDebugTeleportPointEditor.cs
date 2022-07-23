using Safe_To_Share.Scripts.DebugTools;
using UnityEditor;
using UnityEngine;

namespace Spawner.Editor
{
    [CustomEditor(typeof(CreateDebugTeleportPoint))]
    public class CreateDebugTeleportPointEditor : UnityEditor.Editor
    {
        CreateDebugTeleportPoint myTarget;


        void OnEnable() => myTarget = (CreateDebugTeleportPoint)target;

        void OnSceneGUI()
        {
            if (!Application.isEditor) return;

            Event e = Event.current;


            if (e.control && e.type == EventType.MouseDown)
            {
                int controlID = GUIUtility.GetControlID(FocusType.Passive);
                GUIUtility.hotControl = controlID;
                Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, myTarget.validRaycastTargets))
                    myTarget.AddNewPoint(hit.point);

                e.Use();
            }
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("hold control and left click mouse to add point");
            base.OnInspectorGUI();
        }
    }
}