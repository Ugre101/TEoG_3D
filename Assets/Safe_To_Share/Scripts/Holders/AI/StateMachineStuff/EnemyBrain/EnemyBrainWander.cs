using Character.EnemyStuff;
using UnityEngine;

namespace AvatarStuff.Holders.AI.StateMachineStuff.EnemyBrain
{
    public class EnemyBrainWander : State<EnemyAiHolder>
    {
        float range;

        public EnemyBrainWander(EnemyAiHolder enemyAiHolder) : base(enemyAiHolder)
        {
        }

        public override void OnEnter()
        {
            range = 20f;
            for (int i = 0; i < 30; i++)
            {
                Vector3 randomDest = behaviour.SpawnLocation; // Always wander around start pos
                randomDest += new Vector3(Random.Range(-range, range), 20, Random.Range(-range, range));
                if (behaviour.AIMover.AISetDest(randomDest)) 
                    return;
            }

            behaviour.ChangeState(new EnemyBrainIdle(behaviour));
        }

        public override void OnUpdate()
        {
            if (behaviour.DistanceToPlayer <= behaviour.AggroRange)
                behaviour.ChangeState(new EnemyBrainChase(behaviour));
            else if (!behaviour.Move.hasPath)
                behaviour.ChangeState(new EnemyBrainIdle(behaviour));
        }
    }
}