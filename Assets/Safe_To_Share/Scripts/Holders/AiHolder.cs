using Movement.ECM2.Source.Characters;
using Safe_To_Share.Scripts.Holders;
using Safe_To_Share.Scripts.Movement.NavAgentMover;
using Safe_To_Share.Scripts.Static;
using UnityEngine;
using UnityEngine.AI;

namespace AvatarStuff.Holders
{
    public abstract class AiHolder : Holder
    {
        const int FrameLimit = 4;
        [SerializeField] NavMeshAgent move;
        [SerializeField] NavMover aIMover;
        bool outOfRange;
        protected bool Stopped;


        public NavMeshAgent Move
        {
            get => move;
            private set => move = value;
        }

        public NavMover AIMover
        {
            get => aIMover;
            private set => aIMover = value;
        }

        public float DistanceToPlayer { get; private set; } = float.MaxValue;

        protected bool OutOfRange
        {
            get => outOfRange;
            private set
            {
                outOfRange = value;
                if (outOfRange)
                    OutOfRangeFunction();
                else
                    BackInRangeFunction();
            }
        }

        protected virtual void Start()
        {
            if (Move == null && TryGetComponent(out NavMeshAgent agent))
                Move = agent;
            if (!Move.isOnNavMesh)
            {
                Debug.LogWarning("Holder spawned off navmesh", this);
                gameObject.SetActive(false);
            }

            if (AIMover == null && TryGetComponent(out NavMover aiAgent))
                AIMover = aiAgent;
        }

        protected virtual void Update()
        {
            if (Stopped) return;
            if (Time.frameCount % FrameLimit != 0)
                return;

            DistanceToPlayer = Vector3.Distance(transform.position, PlayerHolder.Position);

            OutOfRange = OutOfRange switch
            {
                false when DistanceToPlayer > SpawnSettings.SpawnWhenPlayerAreWithinDistance => true,
                true when DistanceToPlayer <= SpawnSettings.SpawnWhenPlayerAreWithinDistance => false,
                _ => OutOfRange,
            };
        }

        protected virtual void BackInRangeFunction() => transform.AwakeChildren();

        protected virtual void OutOfRangeFunction() => transform.SleepChildren();
    }
}