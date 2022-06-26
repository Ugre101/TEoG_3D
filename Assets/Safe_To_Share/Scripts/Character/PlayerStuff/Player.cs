using System;
using Character.Ailments;
using Character.CreateCharacterStuff;
using Character.Family;
using Character.Organs;
using Character.PlayerStuff.Currency;
using Character.PregnancyStuff;
using Character.VoreStuff;
using Currency;
using Items;
using QuestStuff;
using Static;
using UnityEngine;

namespace Character.PlayerStuff
{
    [Serializable]
    public class Player : ControlledCharacter, ITickDay
    {
        [SerializeField] DateSave lastTimeSlept;

        public Player()
        {
        }

        public Player(CreateCharacter character) : base(character)
        {
            foreach (Item startItem in character.StartItems)
                Inventory.AddItem(startItem);
        }

        public Inventory Inventory { get; } = new();

        public DateSave LastTimeSlept
        {
            get => lastTimeSlept;
            set => lastTimeSlept = value;
        }

        public void TickDay(int ticks = 1) => this.TickPregnancy(ticks);

        public override void Sub()
        {
            base.Sub();
            DateSystem.TickDay += TickDay;
            PlayerIsTheFather += MyChild;
            PlayerQuests.QuestReward += CollectQuestReward;
        }

        void CollectQuestReward(QuestReward obj)
        {
            LevelSystem.GainExp(obj.ExpGain);
            PlayerGold.GoldBag.GainGold(obj.GoldGain);
        }

        void MyChild(Child obj)
        {
            if (obj.FamilyTree.Father.ID == Identity.ID)
                FamilyTree.Children.Add(obj.Identity.ID);
        }

        public override void Unsub()
        {
            base.Unsub();
            DateSystem.TickDay -= TickDay;
            PlayerIsTheFather -= MyChild;
            PlayerQuests.QuestReward -= CollectQuestReward;
        }

        public override void OnBirth(Fetus obj)
        {
            Child newBorn = BaseOnBirth(obj);
            CharacterEvents.CharacterEvents.PlayerEvents.PlayerBirthEvent.StartEvent(this);
            InvokeUpdateAvatar();
        }

        public override void TickMin(int ticks = 1)
        {
            base.TickMin(ticks);
            if (OptionalContent.Vore.Enabled && Vore.VoreTick(this,true, ticks))
                InvokeUpdateAvatar();
        }

        public override void TickHour(int ticks = 1)
        {
            base.TickHour(ticks);
            CheckAilments();
        }

        public void CheckAilments()
        {
            if (this.CheckHungry()) UpdateAilments?.Invoke();

            if (this.CheckTired()) UpdateAilments?.Invoke();
        }

        public event Action UpdateAilments;

        public override void OnStomachDigestion(Prey prey, string digestionMode)
        {
            base.OnStomachDigestion(prey, digestionMode);
            if (CharacterEvents.CharacterEvents.PlayerEvents.VoreEvents.VoreStomachEvents.TryGetValue(digestionMode,
                    out var duoEvent))
                duoEvent.StartEvent(this, prey);
#if UNITY_EDITOR
            else
                Debug.Log($"This stomach digestion {digestionMode} event need to be added");
#endif
        }

        public override void OnOrganDigestion(SexualOrganType organType, Prey prey, string mode)
        {
            if (CharacterEvents.CharacterEvents.PlayerEvents.VoreEvents.VoreOrganEvents.TryGetValue(organType,
                    out var events))
                if (events.TryGetValue(mode, out var duoEvent))
                    duoEvent.StartEvent(this, prey);
#if UNITY_EDITOR
                else
                    Debug.Log($"No {organType.ToString()} digestion {mode} added");
            else

                Debug.Log($"{organType.ToString()} has no digestion events");
#endif
        }

        public override void OnOrganDigestionProgress(SexualOrganType organType, Prey prey, string mode, float progress)
        {
            if (CharacterEvents.CharacterEvents.PlayerEvents.VoreEvents.VoreOrganProgressEvents.TryGetValue(organType,
                    out var events))
                if (events.TryGetValue(mode, out var duoEvent))
                    duoEvent.StartEvent(this, prey);
#if UNITY_EDITOR
                else
                    Debug.Log($"No {organType.ToString()} digestion {mode} added");
            else
                Debug.Log($"{organType.ToString()} has no digestion events");
#endif
        }

        public static event Action<Player,BaseCharacter[]> StartCombat;

        public void InvokeCombat(BaseCharacter[] enemy)
        {
            StartCombat?.Invoke(this,enemy);
        }
    }
}