using System;
using AvatarStuff;
using Character.EnemyStuff;
using Safe_To_Share.Scripts.Holders.AI.StateMachineStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.Holders {
    public sealed class EnemyAiHolder : AiHolder {
        [SerializeField] Enemy enemy;
        [SerializeField] float aggroRange = 10f;
        [Range(0f, 5f), SerializeField,] float initCombatRange = 1.5f;

        StateHandler stateHandler;

        bool waitingToReturn;
        public float AggroRange => aggroRange;
        public Vector3 SpawnLocation { get; private set; }

        public Enemy Enemy => enemy;

        public bool InterActedWith { get; private set; }

        protected override void Start() {
            base.Start();
            SpawnLocation = transform.position;
            Changer.NewAvatar += ModifyAvatar;
            Changer.NewAvatar += NewAvatar;
            stateHandler = new StateHandler(this);
            // Player.RaceSystem.RaceChangedEvent += RaceChange;
        }

        void FixedUpdate() {
            if (Stopped) return;
            if (OutOfRange || Time.frameCount % 3 != 0)
                return;
            if (DistanceToPlayer < initCombatRange)
                StartCombat();
            stateHandler.CurrentState.OnUpdate();
        }

        void OnDestroy() {
            Enemy.Unsub();
            Changer.NewAvatar -= ModifyAvatar;
            Changer.NewAvatar -= NewAvatar;
        }

        void OnTriggerEnter(Collider other) {
            DidIHitPlayer(other);
        }

        void OnTriggerExit(Collider other) {
            if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
                print("Stop swim");
        }

        void OnTriggerStay(Collider other) {
            if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
                print("Start swim");
        }

        public void ModifyAvatar(CharacterAvatar obj) {
            if (waitingToReturn)
                ReturnMe?.Invoke(this);
            else
                obj.Setup(Enemy);
        }

        void StartCombat() => InterActedWith = true;

        //Stopped = true;
        //Player.TriggerCombat(enemy);
        public void ChangeState(StateHandler.States newState) => stateHandler.ChangeState(newState);

        public void AddEnemy(Enemy newEnemy) {
            waitingToReturn = false;
            enemy = newEnemy;
            UpdateAvatar(Enemy);
            HeightsChange(Enemy.Body.Height.Value);
        }

        protected override void NewAvatar(CharacterAvatar obj) {
            if (!enemy.WantBodyMorph) return;
            obj.GetRandomBodyMorphs(enemy);
            ModifyAvatar(obj);
            enemy.WantBodyMorph = false;
        }

        protected override void OutOfRangeFunction() {
            if (Changer.AvatarLoaded)
                ReturnMe?.Invoke(this);
            else
                waitingToReturn = true;
        }

        void DidIHitPlayer(Collider other) {
            if (Enemy.Defeated) return;
            if (!other.gameObject.CompareTag("Player")) return;
            if (!other.TryGetComponent(out PlayerHolder playerHolder)) return;
            playerHolder.TriggerCombat(enemy);
        }

        public event Action<EnemyAiHolder> ReturnMe;
    }
}