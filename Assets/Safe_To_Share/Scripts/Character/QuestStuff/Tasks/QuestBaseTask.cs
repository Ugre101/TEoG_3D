using Safe_to_Share.Scripts.CustomClasses;
using UnityEngine;

namespace Safe_To_Share.Scripts.QuestStuff.Tasks
{
    public class QuestBaseTask : ScriptableObject
    {
        public const string Name = "BaseTask";

        public virtual bool IsCleared()
        {
            return false;
        }
    }
}