using UnityEngine;

namespace Safe_To_Share.Scripts.Holders.AI.StateMachineStuff.EnemyBrain
{
    public sealed class EnemyBrainStopChase : State<EnemyAiHolder>
    {
        float waitUntil;

        public EnemyBrainStopChase(EnemyAiHolder behaviour) : base(behaviour)
        {
        }

        public override void OnEnter()
        {
            waitUntil = Time.time + Random.Range(2f, 5f);
            Behaviour.Agent.ResetPath();
        }

        public override void OnUpdate()
        {
            if (Behaviour.DistanceToPlayer <= Behaviour.AggroRange)
                Behaviour.ChangeState(StateHandler.States.Chase);
            else
                switch (Time.time >= waitUntil)
                {
                    case true when Vector3.Distance(Behaviour.transform.position, Behaviour.SpawnLocation) > 2f:
                        Behaviour.AIMover.SampleAndSetPositionNear(Behaviour.SpawnLocation);
                        break;
                    case true:
                        Behaviour.ChangeState(StateHandler.States.Idle);
                        break;
                }
        }
    }
}