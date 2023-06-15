using System.Collections.Generic;
using AvatarStuff;
using Character;
using Character.EnemyStuff;
using Safe_To_Share.Scripts.Holders.AI.StateMachineStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.Holders.SubRealm {
    public class SubRealmEnemy : AiHolder {
        static readonly HashSet<SubRealmEnemy> ActiveEnemies = new();
        static readonly int SittingOnGround = Animator.StringToHash("Sitting On Ground");
        [field: SerializeField] public Enemy Enemy { get; private set; }
        [field: SerializeField] public float AggroRange { get; private set; } = 10f;
        [SerializeField] float joinCombatRange = 4f;

        State<EnemyAiHolder> currentState;

        public Vector3 SpawnLocation { get; private set; }

        protected override void Start() {
            base.Start();
            Changer.NewAvatar += ModifyAvatar;
            Changer.NewAvatar += NewAvatar;
            SpawnLocation = transform.position;
            ActiveEnemies.Add(this);
            if (Changer.AvatarLoaded)
                ShouldSit(Changer.CurrentAvatar);
        }

        void OnDestroy() {
            Enemy.Unsub();
            ActiveEnemies.Remove(this);
        }

        void OnTriggerEnter(Collider other) {
            if (Enemy.Defeated) return;
            if (!other.gameObject.CompareTag("Player")) return;
            if (!other.TryGetComponent(out PlayerHolder playerHolder)) return;
            List<BaseCharacter> enemies = new() { Enemy, };
            foreach (var subRealmEnemy in ActiveEnemies) {
                if (subRealmEnemy == this) continue;
                if (Vector3.Distance(transform.position, subRealmEnemy.transform.position) < joinCombatRange)
                    enemies.Add(subRealmEnemy.Enemy);
            }

            playerHolder.TriggerSubRealmCombat(enemies.ToArray(), true);
        }

        public void ModifyAvatar(CharacterAvatar obj) => obj.Setup(Enemy);

        protected override void NewAvatar(CharacterAvatar obj) {
            if (Enemy.WantBodyMorph) {
                obj.GetRandomBodyMorphs(Enemy);
                ModifyAvatar(obj);
                Enemy.WantBodyMorph = false;
            }

            ShouldSit(obj);
        }

        void ShouldSit(CharacterAvatar obj) {
            if (Enemy.Defeated)
                obj.Animator.SetBool(SittingOnGround, true);
            // Set defeated state
        }

        public void Setup(Enemy enemy) {
            Enemy = enemy;
            UpdateAvatar(Enemy);
            HeightsChange(Enemy.Body.Height.Value);
        }
    }
}