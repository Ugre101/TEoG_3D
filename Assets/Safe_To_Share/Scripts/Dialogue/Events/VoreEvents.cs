using AvatarStuff.Holders;
using Character.VoreStuff;
using Safe_To_Share.Scripts.Holders;
using UnityEngine;

namespace Dialogue.Events
{
    public sealed class VoreEvents : MonoBehaviour
    {
        [SerializeField] PlayerHolder playerHolder;
        [SerializeField] BaseDialogue pleadDialogue;

        void Start() => VoreOrgan.PleadEvent += StartPleadEvent;

        void OnDestroy() => VoreOrgan.PleadEvent -= StartPleadEvent;

        void StartPleadEvent(Prey arg1, VoreOrgan arg2) =>
            pleadDialogue.TriggerVoreEvent(playerHolder.Player, arg1, arg2);
    }
}