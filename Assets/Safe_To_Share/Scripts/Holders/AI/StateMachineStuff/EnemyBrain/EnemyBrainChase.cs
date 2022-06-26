using Assets.Scripts.Static;
using Character.EnemyStuff;

namespace AvatarStuff.Holders.AI.StateMachineStuff.EnemyBrain
{
    public class EnemyBrainChase : State<EnemyAiHolder>
    {
        public EnemyBrainChase(EnemyAiHolder behaviour) : base(behaviour)
        {
        }

        public override void OnEnter()
        {
            GameManager.EnemyGrowsCloser(GameManager.EnemyClose.Chasing);
        }

        public override void OnUpdate()
        {
            if (behaviour.DistanceToPlayer <= behaviour.AggroRange)
            {
                behaviour.AIMover.MoveToLocation(behaviour.Player.transform.position);
                behaviour.AIMover.Sprint();
            }
            else
                behaviour.ChangeState(new EnemyBrainStopChase(behaviour));
        }

        public override void OnExit() => behaviour.AIMover.StopSprinting();
    }
}