using Safe_To_Share.Scripts.QuestStuff;
using Safe_To_Share.Scripts.QuestStuff.Tasks;
using UnityEditor;
using UnityEngine;

namespace Safe_To_Share.Scripts.Editor.Quest {
    [CustomEditor(typeof(QuestNode))]
    public sealed class QuestNodeEditor : UnityEditor.Editor {
        const string Gather = "Gather", Hunt = "Hunt";

        static readonly string[] Options = {
            Gather,
            Hunt,
        };

        QuestNode myTarget;

        int select;

        void OnEnable() {
            myTarget = (QuestNode)target;
        }

        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            select = EditorGUILayout.Popup(select, Options);
            if (GUILayout.Button("Add Task"))
                AddTask();
            for (var index = myTarget.Tasks.Count - 1; index >= 0; index--) {
                var questBaseTask = myTarget.Tasks[index];
                PrintTask(questBaseTask);
            }
        }

        void PrintTask(QuestBaseTask questBaseTask) {
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField(questBaseTask.GetType().Name);
            TaskButtons(questBaseTask);
            EditorGUILayout.EndVertical();
        }

        void TaskButtons(QuestBaseTask questBaseTask) {
            EditorGUILayout.BeginHorizontal("Box");
            if (GUILayout.Button("Select"))
                Selection.activeObject = questBaseTask;
            if (GUILayout.Button("Remove")) {
                myTarget.RemoveTask(questBaseTask);
                EditorUtility.SetDirty(this);
            }

            EditorGUILayout.EndHorizontal();
        }

        void AddTask() {
            var res = Options[select];
            switch (res) {
                case Gather:
                    myTarget.AddTask<QuestGatherTask>();
                    break;
                case Hunt:
                    myTarget.AddTask<QuestHuntTask>();
                    break;
            }
        }
    }
}