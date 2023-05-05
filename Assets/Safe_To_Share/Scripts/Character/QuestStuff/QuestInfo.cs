using Safe_to_Share.Scripts.CustomClasses;
using UnityEngine;

namespace QuestStuff
{
    [CreateAssetMenu(fileName = "Quest info", menuName = "ScriptableObject/Quest")]
    public class QuestInfo : SObjSavableTitleDescIcon
    {
        [SerializeField] string world;
        [SerializeField] int progressGoal;
        [SerializeField] bool questChain;

        [Header("Reward"), SerializeField,] bool hasInstantReward;
        [SerializeField] QuestReward questReward;
        [SerializeField, HideInInspector,] string returnGuid;
        [SerializeField, HideInInspector,] string nextGuid;
        public string World => world;
        public int ProgressGoal => progressGoal;
        public string ReturnGuid => returnGuid;
        public string NextGuid => nextGuid;

        public bool QuestChain => questChain;

        public bool HasInstantReward => hasInstantReward;

        public QuestReward QuestReward => questReward;

#if UNITY_EDITOR
        [Header("Editor Only"), SerializeField,]
        QuestReturnInfo returnTo;

        [SerializeField] QuestInfo altNextQuest;
        public override void OnValidate()
        {
            base.OnValidate();
            returnGuid = returnTo != null ? returnTo.Guid : string.Empty;
            if (altNextQuest != null)
            {
                nextGuid = altNextQuest.Guid;
                questChain = true;
            }
            else
            {
                nextGuid = string.Empty;
                questChain = false;
            }
        }
#endif
    }
}