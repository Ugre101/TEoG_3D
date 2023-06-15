using System;
using UnityEngine;

namespace QuestStuff {
    [Serializable]
    public class QuestProgress {
        [SerializeField] string questId;
        [SerializeField] int progress;

        QuestInfo info;

        public QuestProgress(QuestInfo questInfo, int progress = 0) {
            info = questInfo;
            questId = questInfo.Guid;
            this.progress = progress;
        }

        public string QuestId => questId;
        public int Progress => progress;

        public bool ProgressQuest(int by) {
            progress += by;
            return progress >= info.ProgressGoal;
        }
    }
}