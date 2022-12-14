using AvatarStuff;
using AvatarStuff.Holders;
using AvatarStuff.Holders.AI.StateMachineStuff;
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

        protected override void Start()
        {
            base.Start();
            SpawnLocation = transform.position;
            Changer.NewAvatar += ModifyAvatar;
            Changer.NewAvatar += NewAvatar;
        }
        void OnDestroy()
        {
            Enemy.Unsub();
            Changer.NewAvatar -= ModifyAvatar;
            Changer.NewAvatar -= NewAvatar;
        }
        void ModifyAvatar(CharacterAvatar obj)
        {
            obj.Setup(Enemy);
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