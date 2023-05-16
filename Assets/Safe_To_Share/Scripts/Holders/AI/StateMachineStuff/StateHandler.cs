using System;
using AvatarStuff.Holders;
using Safe_To_Share.Scripts.Holders.AI.StateMachineStuff.EnemyBrain;
using UnityEngine;

namespace Safe_To_Share.Scripts.Holders.AI.StateMachineStuff
{
    public class StateHandler
    {
        readonly EnemyBrainIdle idle;
        readonly EnemyBrainWander wander;
        readonly EnemyBrainChase chase;
        readonly EnemyBrainStopChase stopChase;

        public StateHandler(EnemyAiHolder holder)
        {
            idle = new EnemyBrainIdle(holder);
            wander = new EnemyBrainWander(holder);
            chase = new EnemyBrainChase(holder);
            stopChase = new EnemyBrainStopChase(holder);
            CurrentState = idle;
        }
        
        public enum  States
        {
            Idle,
            Wander,
            Chase,
            StopChase,
        }

        public State<EnemyAiHolder> CurrentState { get; private set; }
        public void ChangeState(States newState)
        {
            CurrentState?.OnExit();
            CurrentState = newState switch
            {
                States.Idle => idle,
                States.Wander => wander,
                States.Chase => chase,
                States.StopChase => stopChase,
                _ => throw new ArgumentOutOfRangeException(nameof(newState), newState, null)
            };
            CurrentState.OnEnter();
        }
    }
}