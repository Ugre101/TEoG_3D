using System.Collections;
using UnityEngine;

namespace Safe_To_Share.Scripts.Movement.HoverMovement.PhysicsLayer
{
    public class RoadLayer : BaseLayer
    {
        [SerializeField, Range(0.5f, 3f),] float removeDelay = 1f;
        [SerializeField] FloatMod speedMod;

        Coroutine removeRoutine;

        WaitForSeconds waitForSeconds;
        void Start() => waitForSeconds = new WaitForSeconds(removeDelay);

        public override void OnEnter(Movement mover)
        {
            if (removeRoutine is not null)
                StopCoroutine(removeRoutine);
            else
                mover.Stats.AddMod(MoveCharacter.MoveModes.Walking, speedMod);
        }

        public override void OnExit(Movement mover)
        {
            removeRoutine = StartCoroutine(RemoveAfterDelay(mover.Stats));
        }

        public override void OnFixedUpdate(Movement movement)
        {
        }

        IEnumerator RemoveAfterDelay(MoveStats moveStatsManager)
        {
            yield return waitForSeconds;
            moveStatsManager.RemoveMod(MoveCharacter.MoveModes.Walking, speedMod);
            removeRoutine = null;
        }
    }
}