using Safe_To_Share.Scripts.Movement.HoverMovement.PhysicsLayer;
using UnityEngine;

namespace Safe_To_Share.Scripts.Movement.HoverMovement
{
    public sealed class PhysicLayerHandler
    {
        BaseLayer currentLayer;
        bool hasLayer;

        public void OnFixedUpdate(Movement movement, bool grounded, Collider lastHitCollider)
        {
            if (grounded)
                TryEnterPhysicsLayer(movement, lastHitCollider);
            else 
                TryExitPhysicsLayer(movement);
            if (hasLayer)
                currentLayer.OnFixedUpdate(movement);
        }

         void TryEnterPhysicsLayer(Movement movement, Component other)
        {
            if (!other.gameObject.TryGetComponent(out BaseLayer baseLayer))
            {
                TryExitPhysicsLayer(movement);
                return;
            }
            if (hasLayer) 
                currentLayer.OnExit(movement);
            currentLayer = baseLayer;
            hasLayer = true;
            baseLayer.OnEnter(movement);
        }

         void TryExitPhysicsLayer(Movement movement)
        {
            if (hasLayer is false)
                return;
            currentLayer.OnExit(movement);
            currentLayer = null;
            hasLayer = false;
        }
    }
}