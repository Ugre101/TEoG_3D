using CustomClasses;
using UnityEngine;

namespace Safe_To_Share.Scripts.QuestStuff
{
    [CreateAssetMenu(fileName = "New Quest", menuName = "ScriptableObject/Quest 2", order = 0)]
    public sealed class Quest : BaseEditorCanvasObject<QuestNode>
    {

        public void Save()
        {
            foreach (var questNode in nodes)
            {
                
            } 
        }

        public void Load()
        {
            
        }

        public void CheckProgress()
        {
        }
    }
}