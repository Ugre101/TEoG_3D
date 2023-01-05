using System;
using System.Collections.Generic;
using AvatarStuff;
using AvatarStuff.Holders;
using AvatarStuff.Holders.AI.StateMachineStuff;
using Character;
using Character.EnemyStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.Holders
{
    public class SubRealmEnemy : AiHolder
    {
        [Range(0f, 5f), SerializeField,] float initCombatRange = 1.5f;
        [field: SerializeField] public Enemy Enemy { get; private set; }
        [field: SerializeField] public float AggroRange { get; private set; } = 10f;

        State<EnemyAiHolder> currentState;

        public Vector3 SpawnLocation { get; private set; }

        public bool InterActedWith { get; private set; }

        static readonly HashSet<SubRealmEnemy> ActiveEnemies = new();
        [SerializeField] float joinCombatRange = 4f;

        protected override void Start()
        {
            base.Start();
            SpawnLocation = transform.position;
            Changer.NewAvatar += ModifyAvatar;
            Changer.NewAvatar += NewAvatar;
            ActiveEnemies.Add(this);
        }
        void OnDestroy()
        {
            Enemy.Unsub();
            Changer.NewAvatar -= ModifyAvatar;
            Changer.NewAvatar -= NewAvatar;
            ActiveEnemies.Remove(this);
        }
        void ModifyAvatar(CharacterAvatar obj)
        {
            obj.Setup(Enemy);
        }

        void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            if (!other.TryGetComponent(out PlayerHolder playerHolder)) return;
            List<BaseCharacter> enemies = new(){Enemy, };
            foreach (var subRealmEnemy in ActiveEnemies)
            {
                if (subRealmEnemy == this) continue;
                if (Vector3.Distance(transform.position, subRealmEnemy.transform.position) < joinCombatRange)
                    enemies.Add(subRealmEnemy.Enemy);
            }
            playerHolder.TriggerCombat(enemies.ToArray());
        }

        protected override void NewAvatar(CharacterAvatar obj)
        {
            if (Enemy.WantBodyMorph)
            {
                obj.GetRandomBodyMorphs(Enemy);
                ModifyAvatar(obj);
                Enemy.WantBodyMorph = false;
            }
        }

        public void Setup(Enemy enemy)
        {
            Enemy = enemy;
            UpdateAvatar(Enemy);
            HeightsChange(Enemy.Body.Height.Value);
        }
    }
}