using Movement.ECM2.Source.Characters;
using UnityEngine;
using UnityEngine.Events;

namespace Safe_To_Share.Scripts
{
    public class InvokeEventOnEnterExit : MonoBehaviour
    {
        // [SerializeField] DormSleepAreaShared dorm;
        [SerializeField] UnityEvent enterEvent;
        [SerializeField] UnityEvent exitEvent;

        [SerializeField] bool stopMoverOnEnter;
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                enterEvent.Invoke();
            if (stopMoverOnEnter && other.TryGetComponent(out ThirdPersonEcm2Character mover))
            {
                mover.Stop();
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
                exitEvent.Invoke();    
        }
    }
}