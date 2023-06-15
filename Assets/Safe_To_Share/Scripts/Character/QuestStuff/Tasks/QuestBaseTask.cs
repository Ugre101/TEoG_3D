using UnityEngine;

namespace Safe_To_Share.Scripts.QuestStuff.Tasks {
    public class QuestBaseTask : ScriptableObject {
        public const string Name = "BaseTask";

        public virtual bool IsCleared() => false;
    }
}