using QuestStuff;
using UnityEngine;

namespace Dialogue
{
    public class DialogueQuestNode : DialogueBaseNode
    {
        [SerializeField] QuestInfo questQuest;

        public QuestInfo Quest => questQuest;
        public override bool ShowNode => !PlayerQuests.HasQuest(questQuest);
    }
}