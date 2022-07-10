using System;
using AvatarStuff.Holders.AI.StateMachineStuff;
using AvatarStuff.Holders.AI.StateMachineStuff.EnemyBrain;
using Character;
using Character.EnemyStuff;
using UnityEngine;

namespace AvatarStuff.Holders
{
    public class EnemyAiHolder : AiHolder
    {
        [SerializeField] Enemy enemy;
        [SerializeField] float aggroRange = 10f;
        [Range(0f, 5f), SerializeField,] float initCombatRange = 1.5f;

        State<EnemyAiHolder> currentState;
        public float AggroRange => aggroRange;
        public Vector3 SpawnLocation { get; private set; }

        public Enemy Enemy => enemy;

        public bool InterActedWith { get; private set; }

        protected override void Start()
        {
            base.Start();
            SpawnLocation = transform.position;
            Changer.NewAvatar += ModifyAvatar;
            Changer.NewAvatar += NewAvatar;
            // Player.RaceSystem.RaceChangedEvent += RaceChange;
        }

        void FixedUpdate()
        {
            if (Stopped) return;
            if (OutOfRange || Time.frameCount % 3 != 0)
                return;
            if (DistanceToPlayer < initCombatRange)
            {
                StartCombat();
            }
            if (currentState == null)
                ChangeState(new EnemyBrainIdle(this));
            else
                currentState.OnUpdate();
        }

        void OnDestroy()
        {
            Enemy.Unsub();
            Changer.NewAvatar -= ModifyAvatar;
            Changer.NewAvatar -= NewAvatar;
        }

        void ModifyAvatar(CharacterAvatar obj)
        {
            if (waitingToReturn)
                ReturnMe?.Invoke(this);
            else
                obj.Setup(Enemy);
        }

        void StartCombat()
        {
            InterActedWith = true;
            //Stopped = true;
            //Player.TriggerCombat(enemy);
        }

        public void ChangeState(State<EnemyAiHolder> newState)
        {
            currentState?.OnExit();
            currentState = newState;
            currentState.OnEnter();
        }

        public void AddEnemy(Enemy newEnemy)
        {
            waitingToReturn = false;
            enemy = newEnemy;
            UpdateAvatar(Enemy);
            HeightsChange(Enemy.Body.Height.Value);
        }

        protected override void NewAvatar(CharacterAvatar obj)
        {
            if (enemy.WantBodyMorph)
            {
                obj.GetRandomBodyMorphs(enemy);
                ModifyAvatar(obj);
                enemy.WantBodyMorph = false;
            }
        }

        bool waitingToReturn;
        protected override void OutOfRangeFunction()
        {
            if (Changer.AvatarLoaded)
                ReturnMe?.Invoke(this);
            else
                waitingToReturn = true;
        }

        public event Action<EnemyAiHolder> ReturnMe;
    }
}