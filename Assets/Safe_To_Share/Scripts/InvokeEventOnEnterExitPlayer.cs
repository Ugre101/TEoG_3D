using System;
using AvatarStuff.Holders;
using Character.PlayerStuff;
using Safe_To_Share.Scripts.Holders;
using UnityEngine;
using UnityEngine.Events;

namespace Safe_To_Share.Scripts
{
    public class InvokeEventOnEnterExitPlayer : MonoBehaviour
    {
        [SerializeField] PlayerEvent enterEvent;
        [SerializeField] UnityEvent exitEvent;

        [SerializeField] bool stopMoverOnEnter;

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && other.TryGetComponent(out PlayerHolder holder))
            {
                enterEvent.Invoke(holder.Player);
                if (stopMoverOnEnter)
                    holder.PersonEcm2Character.Stop();
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
                exitEvent.Invoke();
        }

        [Serializable]
        class PlayerEvent : UnityEvent<Player>
        {
        }
    }
}