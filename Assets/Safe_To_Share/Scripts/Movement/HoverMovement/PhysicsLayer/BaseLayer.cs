using UnityEngine;

namespace Safe_To_Share.Scripts.Movement.HoverMovement.PhysicsLayer {
    public abstract class BaseLayer : MonoBehaviour {
        public abstract void OnEnter(Movement mover);
        public abstract void OnExit(Movement mover);
        public abstract void OnFixedUpdate(Movement movement);
    }
}