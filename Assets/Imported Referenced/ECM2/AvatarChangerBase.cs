using System;
using UnityEngine;
using UnityEngine.Events;

namespace Movement.ECM2.Source
{
    public abstract class AvatarChangerBase : MonoBehaviour
    {
        public UnityEvent<Animator> newAnimatorEvent;
        protected void InvokeNewAnimator(Animator newAnimator)
        {
            newAnimatorEvent?.Invoke(newAnimator);
        }
    }
}