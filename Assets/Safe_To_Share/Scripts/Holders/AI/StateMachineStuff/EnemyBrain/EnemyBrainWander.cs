using AvatarStuff.Holders;
using UnityEngine;

namespace Safe_To_Share.Scripts.Holders.AI.StateMachineStuff.EnemyBrain
{
    public sealed class EnemyBrainWander : State<EnemyAiHolder>
    {
        const float Range = 20f;

        public EnemyBrainWander(EnemyAiHolder enemyAiHolder) : base(enemyAiHolder)
        {
        }

        public override void OnEnter()
        {
            for (int i = 0; i < 30; i++)
            {
                Vector3 randomDest = Behaviour.SpawnLocation; // Always wander around start pos
                randomDest += Random.insideUnitSphere * Range; // new Vector3(Random.Range(-range, range), 20, Random.Range(-range, range));
                if (Behaviour.AIMover.SampleAndSetPositionNear(randomDest))
                    return;
            }

            Behaviour.ChangeState(StateHandler.States.Idle);
        }

        public override void OnUpdate()
        {
            if (Behaviour.DistanceToPlayer <= Behaviour.AggroRange)
                Behaviour.ChangeState(StateHandler.States.Chase);
            else if (!Behaviour.Move.hasPath)
                Behaviour.ChangeState(StateHandler.States.Idle);
        }
    }
}