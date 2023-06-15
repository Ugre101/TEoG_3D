using Safe_to_Share.Scripts.CustomClasses;
using UnityEngine;

namespace QuestStuff {
    [CreateAssetMenu(fileName = "Quest return info", menuName = "ScriptableObject/QuestReturn")]
    public sealed class QuestReturnInfo : SObjSavableTitleDescIcon {
        [SerializeField] string returnTo;
        public string ReturnTo => returnTo;
    }
}