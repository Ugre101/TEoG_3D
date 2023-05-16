using AvatarStuff.Holders;
using UnityEngine;

namespace Safe_To_Share.Scripts.Holders.AI.StateMachineStuff.EnemyBrain
{
    public sealed class EnemyBrainIdle : State<EnemyAiHolder>
    {
        float waitUntil;

        public EnemyBrainIdle(EnemyAiHolder aiHolder) : base(aiHolder)
        {
        }

        public override void OnEnter()
        {
            waitUntil = Time.time + Random.Range(3f, 10f);
            Behaviour.Move.ResetPath();
        }

        public override void OnUpdate()
        {
            if (Behaviour.DistanceToPlayer <= Behaviour.AggroRange)
                Behaviour.ChangeState(StateHandler.States.Chase);
            else if (Time.time >= waitUntil)
                Behaviour.ChangeState(StateHandler.States.Wander);
        }
    }
}