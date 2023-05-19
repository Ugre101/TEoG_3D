using System.Collections.Generic;
using System.Linq;
using Safe_to_Share.Scripts.CustomClasses;
using Safe_To_Share.Scripts.QuestStuff.Tasks;
using UnityEditor;
using UnityEngine;

namespace Safe_To_Share.Scripts.QuestStuff
{
    public sealed class QuestNode : BaseEditorCanvasNode
#if UNITY_EDITOR
                                    , ISerializationCallbackReceiver
#endif
    {
        [field: SerializeField] public List<QuestBaseTask> Tasks { get; private set; } = new();
#if UNITY_EDITOR
        
        public void OnBeforeSerialize()
        {
            if (string.IsNullOrWhiteSpace(AssetDatabase.GetAssetPath(this))) return;
            foreach (var task in Tasks.Where(questTask =>
                         string.IsNullOrWhiteSpace(AssetDatabase.GetAssetPath(questTask))))
                AssetDatabase.AddObjectToAsset(task, this);
        }
        

        public void OnAfterDeserialize()
        {
        }

        public void AddTask<T>() where  T : QuestBaseTask
        {
            var newNode = CreateInstance<T>();
            newNode.name = System.Guid.NewGuid().ToString();
            Tasks.Add(newNode);
        }

        public void RemoveTask(QuestBaseTask task)
        {
            if (Tasks.Remove(task))
            {
                AssetDatabase.RemoveObjectFromAsset(task);
            }
        }
#endif
    }
}