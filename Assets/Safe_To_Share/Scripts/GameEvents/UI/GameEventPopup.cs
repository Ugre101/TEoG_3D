using Dialogue;
using TMPro;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameEvents.UI {
    public sealed class GameEventPopup : MonoBehaviour {
        [SerializeField] TextMeshProUGUI title, desc;

        public void Setup(BaseDialogue dialogue) { }
    }
}