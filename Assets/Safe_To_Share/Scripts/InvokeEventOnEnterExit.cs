using Safe_To_Share.Scripts.CustomClasses;
using UnityEngine;
using UnityEngine.Events;

namespace Safe_To_Share.Scripts {
    public sealed class InvokeEventOnEnterExit : MonoBehaviour {
        // [SerializeField] DormSleepAreaShared dorm;
        [SerializeField] UnityEvent enterEvent;
        [SerializeField] UnityEvent exitEvent;

        [SerializeField] bool stopMoverOnEnter;

        [SerializeField] bool hasTriggerCondition;
        [SerializeField] TriggerCondition triggerCondition;
        void OnTriggerEnter(Collider other) {
            if (hasTriggerCondition && triggerCondition.ShouldTrigger() is false)
                return;
            if (other.CompareTag("Player"))
                enterEvent.Invoke();
            if (stopMoverOnEnter && other.TryGetComponent(out Movement.HoverMovement.Movement mover)) mover.Stop();
        }

        void OnTriggerExit(Collider other) {
            if (other.CompareTag("Player"))
                exitEvent.Invoke();
        }
    }
}