using Dialogue;
using UnityEngine;

namespace Safe_To_Share.Scripts.Dialogue {
    public sealed class TriggerDialogue : MonoBehaviour {

        [SerializeField] BaseDialogue dialogue;
        public void Trigger() {
            dialogue.StartTalking();
        }
    }
}