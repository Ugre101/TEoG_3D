using System;
using Safe_To_Share.Scripts.CustomClasses;
using Safe_To_Share.Scripts.Holders;
using UnityEngine;
using UnityEngine.Events;

namespace Safe_To_Share.Scripts {
    public sealed class InvokeEventOnEnterExitPlayerHolder : MonoBehaviour {
        [SerializeField] PlayerHolderEvent enterEvent;
        [SerializeField] UnityEvent exitEvent;

        [SerializeField] bool stopMoverOnEnter;
        [SerializeField] bool hasTriggerCondition;
        [SerializeField] TriggerCondition triggerCondition;
        void OnTriggerEnter(Collider other) {
            if (hasTriggerCondition && triggerCondition.ShouldTrigger() is false)
                return;
            if (!other.CompareTag("Player") || !other.TryGetComponent(out PlayerHolder holder)) return;
            enterEvent.Invoke(holder);
            if (stopMoverOnEnter)
                holder.PersonEcm2Character.Stop();
        }

        void OnTriggerExit(Collider other) {
            if (other.CompareTag("Player"))
                exitEvent.Invoke();
        }

        [Serializable]
        class PlayerHolderEvent : UnityEvent<PlayerHolder> { }
    }
}