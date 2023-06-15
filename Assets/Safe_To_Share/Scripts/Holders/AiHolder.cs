using Safe_To_Share.Scripts.Movement.NavAgentMover;
using Safe_To_Share.Scripts.Static;
using UnityEngine;
using UnityEngine.AI;

namespace Safe_To_Share.Scripts.Holders {
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class AiHolder : Holder {
        const int FrameLimit = 4;
        [SerializeField] NavMeshAgent move;
        [SerializeField] NavMover aIMover;
        bool outOfRange;
        protected bool Stopped;


        public NavMeshAgent Agent {
            get => move;
            private set => move = value;
        }

        public NavMover AIMover {
            get => aIMover;
            private set => aIMover = value;
        }

        public float DistanceToPlayer { get; private set; } = float.MaxValue;

        protected bool OutOfRange {
            get => outOfRange;
            private set {
                outOfRange = value;
                if (outOfRange)
                    OutOfRangeFunction();
                else
                    BackInRangeFunction();
            }
        }

        protected virtual void Start() {
            if (Agent == null) {
                if (TryGetComponent(out NavMeshAgent agent)) {
                    Agent = agent;
                } else {
                    Debug.LogWarning($"{gameObject.name} is missing a {nameof(NavMeshAgent)}");
                    enabled = false;
                    return;
                }
            }

            if (!Agent.isOnNavMesh) {
                Debug.LogWarning("Holder spawned off navmesh", this);
                gameObject.SetActive(false);
            }

            if (AIMover == null && TryGetComponent(out NavMover aiAgent))
                AIMover = aiAgent;
        }

        protected virtual void Update() {
            if (Stopped) return;
            if (Time.frameCount % FrameLimit != 0)
                return;

            DistanceToPlayer = Vector3.Distance(transform.position, PlayerHolder.Position);

            OutOfRange = OutOfRange switch {
                false when DistanceToPlayer > SpawnSettings.SpawnWhenPlayerAreWithinDistance => true,
                true when DistanceToPlayer <= SpawnSettings.SpawnWhenPlayerAreWithinDistance => false,
                _ => OutOfRange,
            };
        }
#if UNITY_EDITOR

        protected virtual void OnValidate() {
            if (aIMover == null && TryGetComponent(out aIMover) is false)
                throw new MissingComponentException($"Missing {nameof(NavMover)}");
            if (move == null && TryGetComponent(out move) is false)
                throw new MissingComponentException($"Missing {nameof(NavMeshAgent)}");
        }
#endif

        protected virtual void BackInRangeFunction() => transform.AwakeChildren();

        protected virtual void OutOfRangeFunction() => transform.SleepChildren();
    }
}