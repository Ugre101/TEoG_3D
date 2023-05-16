using UnityEngine;

namespace Safe_To_Share.Scripts.Holders.AI.StateMachineStuff
{
    public abstract class State<TMonoBehavior> where TMonoBehavior : MonoBehaviour
    {
        protected readonly TMonoBehavior Behaviour;

        protected State(TMonoBehavior behaviour) => Behaviour = behaviour;

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