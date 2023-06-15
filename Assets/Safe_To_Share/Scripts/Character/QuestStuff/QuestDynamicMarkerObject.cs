using UnityEngine;

namespace QuestStuff {
    public sealed class QuestDynamicMarkerObject : MonoBehaviour {
        [SerializeField] QuestInfo questInfo;

        public Sprite Icon => questInfo.Icon;

        //  public bool Active => PlayerQuests.Quests.TryGetValue(questInfo, out QuestProgress quest) && quest.Active;
        public Vector3 GetPos() => transform.position;
    }
}