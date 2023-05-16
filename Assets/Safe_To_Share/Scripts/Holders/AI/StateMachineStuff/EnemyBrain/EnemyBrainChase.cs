using AvatarStuff.Holders;
using Safe_To_Share.Scripts.Static;

namespace Safe_To_Share.Scripts.Holders.AI.StateMachineStuff.EnemyBrain
{
    public sealed class EnemyBrainChase : State<EnemyAiHolder>
    {
        public EnemyBrainChase(EnemyAiHolder behaviour) : base(behaviour)
        {
        }

        public override void OnEnter() => GameManager.EnemyGrowsCloser(GameManager.EnemyClose.Chasing);

        public override void OnUpdate()
        {
            if (Behaviour.DistanceToPlayer <= Behaviour.AggroRange)
            {
                Behaviour.AIMover.SampleAndSetPositionNear(PlayerHolder.Position);
                Behaviour.AIMover.SetSprint(true);
            }
            else
                Behaviour.ChangeState(StateHandler.States.Chase);
        }

        public override void OnExit() => Behaviour.AIMover.SetSprint(false);
    }
}