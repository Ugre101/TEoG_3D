using UnityEngine;

namespace AvatarStuff.Holders.AI.StateMachineStuff
{
    public abstract class State<TMonoBehavior> where TMonoBehavior : MonoBehaviour
    {
        protected TMonoBehavior behaviour;

        public State(TMonoBehavior behaviour) => this.behaviour = behaviour;

        public virtual void OnEnter()
        {
        }

        public virtual void OnUpdate()
        {
        }

        public virtual void OnExit()
        {
        }
    }
}