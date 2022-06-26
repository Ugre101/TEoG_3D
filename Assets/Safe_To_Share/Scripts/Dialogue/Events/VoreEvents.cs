using System;
using AvatarStuff.Holders;
using Character.VoreStuff;
using UnityEngine;

namespace Dialogue.Events
{
    public class VoreEvents : MonoBehaviour
    {
        [SerializeField] PlayerHolder playerHolder;
        [SerializeField] BaseDialogue pleadDialogue;

        void Start()
        {
            VoreOrgan.PleadEvent += StartPleadEvent;
        }

        void OnDestroy()
        {
            VoreOrgan.PleadEvent -= StartPleadEvent;
        }

        void StartPleadEvent(Prey arg1, VoreOrgan arg2)
        {
            pleadDialogue.TriggerVoreEvent(playerHolder.Player,arg1,arg2);
        }
    }
}