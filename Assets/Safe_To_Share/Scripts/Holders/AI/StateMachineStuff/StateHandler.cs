using System;
using Safe_To_Share.Scripts.Holders.AI.StateMachineStuff.EnemyBrain;

namespace Safe_To_Share.Scripts.Holders.AI.StateMachineStuff {
    public sealed class StateHandler {
        public enum States {
            Idle,
            Wander,
            Chase,
            StopChase,
        }

        readonly EnemyBrainChase chase;
        readonly EnemyBrainIdle idle;
        readonly EnemyBrainStopChase stopChase;
        readonly EnemyBrainWander wander;

        public StateHandler(EnemyAiHolder holder) {
            idle = new EnemyBrainIdle(holder);
            wander = new EnemyBrainWander(holder);
            chase = new EnemyBrainChase(holder);
            stopChase = new EnemyBrainStopChase(holder);
            CurrentState = idle;
        }

        public State<EnemyAiHolder> CurrentState { get; private set; }

        public void ChangeState(States newState) {
            CurrentState?.OnExit();
            CurrentState = newState switch {
                States.Idle => idle,
                States.Wander => wander,
                States.Chase => chase,
                States.StopChase => stopChase,
                _ => throw new ArgumentOutOfRangeException(nameof(newState), newState, null),
            };
            CurrentState.OnEnter();
        }
    }
}