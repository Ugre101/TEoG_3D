using UnityEngine;

namespace AvatarStuff.Holders.AI.StateMachineStuff.EnemyBrain
{
    public class EnemyBrainIdle : State<EnemyAiHolder>
    {
        float waitUntil;

        public EnemyBrainIdle(EnemyAiHolder aiHolder) : base(aiHolder)
        {
        }

        public override void OnEnter()
        {
            waitUntil = Time.time + Random.Range(3f, 10f);
            behaviour.Move.ResetPath();
        }

        public override void OnUpdate()
        {
            if (behaviour.DistanceToPlayer <= behaviour.AggroRange)
                behaviour.ChangeState(new EnemyBrainChase(behaviour));
            else if (Time.time >= waitUntil)
                behaviour.ChangeState(new EnemyBrainWander(behaviour));
        }
    }

    public class EnemyBrainStopChase : State<EnemyAiHolder>
    {
        float waitUntil;

        public EnemyBrainStopChase(EnemyAiHolder behaviour) : base(behaviour)
        {
        }

        public override void OnEnter()
        {
            waitUntil = Time.time + Random.Range(2f, 5f);
            behaviour.Move.ResetPath();
        }

        public override void OnUpdate()
        {
            if (behaviour.DistanceToPlayer <= behaviour.AggroRange)
                behaviour.ChangeState(new EnemyBrainChase(behaviour));
            else
                switch (Time.time >= waitUntil)
                {
                    case true when Vector3.Distance(behaviour.transform.position, behaviour.SpawnLocation) > 2f:
                        behaviour.AIMover.AISetDest(behaviour.SpawnLocation);
                        break;
                    case true:
                        behaviour.ChangeState(new EnemyBrainIdle(behaviour));
                        break;
                }
        }
    }
}