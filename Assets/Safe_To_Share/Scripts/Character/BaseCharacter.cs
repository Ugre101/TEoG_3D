using System;
using System.Collections;
using System.Collections.Generic;
using Character.BodyStuff;
using Character.CreateCharacterStuff;
using Character.EssenceStuff;
using Character.Family;
using Character.GenderStuff;
using Character.IdentityStuff;
using Character.IslandData;
using Character.LevelStuff;
using Character.Organs;
using Character.PregnancyStuff;
using Character.Race;
using Character.Race.Races;
using Character.RelationShipStuff;
using Character.SexStatsStuff;
using Character.StatsStuff;
using Character.VoreStuff;
using Static;
using UnityEngine;

namespace Character
{
    [Serializable]
    public abstract class BaseCharacter : ITickMinute
    {
        [SerializeField] Identity identity;
        [SerializeField] FamilyTree familyTree;
        [SerializeField] Stats stats = new();
        [SerializeField] Body body = new();
        [SerializeField] VoreSystem vore = new();
        [SerializeField] SexualOrgans sexualOrgans = new();
        [SerializeField] CharacterLevel level = new();
        [SerializeField] EssenceSystem essenceSystem = new();
        [SerializeField] SexStats sexStats = new();
        [SerializeField] PregnancySystem pregnancySystem = new();
        [SerializeField] RelationsShips relationsShips = new();
        [SerializeField] Hair hair = new(false, Color.white);
        Gender lastGender;

        protected BaseCharacter(BaseCharacter oldCharacter)
        {
            identity = oldCharacter.Identity;
            stats = oldCharacter.Stats;
            body = oldCharacter.Body;
            vore = oldCharacter.Vore;
            sexualOrgans = oldCharacter.SexualOrgans;
            level = oldCharacter.LevelSystem;
            essenceSystem = oldCharacter.Essence;
            sexStats = oldCharacter.SexStats;
            RaceSystem = oldCharacter.RaceSystem;
            familyTree = oldCharacter.FamilyTree;
            pregnancySystem = oldCharacter.PregnancySystem;
            hair = oldCharacter.Hair;
        }

        protected BaseCharacter(CreateCharacter character)
        {
            identity = character.Identity.GetIdentity(Gender.GetGenderType());
            stats = character.Stats;
            body = character.StartBody.NewBody();
            vore = new VoreSystem();
            pregnancySystem = new PregnancySystem();
            character.StartGender.SetGender(this);
            lastGender = Gender;
            RaceSystem.AddRace(character.StartRace);
            RaceChange(null, character.StartRace);
            familyTree = new FamilyTree();
            if (character.StartPerks != null)
                foreach (BasicPerk startPerk in character.StartPerks)
                {
                    LevelSystem.OwnedPerks.Add(startPerk);
                    startPerk.PerkGainedEffect(this);
                }

            Stats.FullRecovery();
            hair = character.Hair.GetHair();
            body.SkinTone = character.Skin.GetSkinDarkness();
        }

        protected BaseCharacter(CreateCharacter character, Islands islands)
        {
            identity = character.Identity.GetIdentity(Gender.GetGenderType());
            stats = character.Stats;
            body = character.StartBody.NewBody(islands);
            vore = new VoreSystem();
            pregnancySystem = new PregnancySystem();
            character.StartGender.SetGender(this, islands);
            lastGender = Gender;
            RaceSystem.AddRace(character.StartRace);
            RaceChange(null, character.StartRace);
            familyTree = new FamilyTree();
            if (character.StartPerks != null)
                foreach (BasicPerk startPerk in character.StartPerks)
                {
                    LevelSystem.OwnedPerks.Add(startPerk);
                    startPerk.PerkGainedEffect(this);
                }

            Stats.FullRecovery();
            hair = character.Hair.GetHair();
            body.SkinTone = character.Skin.GetSkinDarkness();
        }

        protected BaseCharacter()
        {
            identity = new Identity();
            familyTree = new FamilyTree();
        }

        public Identity Identity => identity;
        public Stats Stats => stats;
        public Body Body => body;
        public VoreSystem Vore => vore;
        public RaceSystem RaceSystem { get; private set; } = new();
        public SexualOrgans SexualOrgans => sexualOrgans;
        public CharacterLevel LevelSystem => level;
        public EssenceSystem Essence => essenceSystem;

        public Gender Gender => GenderSettings.GetGender(this);

        public SexStats SexStats => sexStats;

        public FamilyTree FamilyTree => familyTree;

        public PregnancySystem PregnancySystem => pregnancySystem;

        public RelationsShips RelationsShips => relationsShips;

        public Hair Hair => hair;

        public virtual void TickMin(int ticks = 1)
        {
            Stats.TickMin(ticks);
            SexualOrgans.TickMin(ticks);
        }

        public virtual void TickHour(int ticks = 1)
        {
            Stats.TickHour(ticks);
            PregnancySystem.TickHour(ticks);
            Vore.TickHour(this, ticks);
            essenceSystem.TickHour(ticks);
            bool modifyAvatar = TickHourIfChangeModifyAvatar(ticks);
            SexStats.TickHour(ticks);
            Body.BurnFatHour(ticks);
            if (modifyAvatar)
                InvokeUpdateAvatar();
        }

        protected virtual bool TickHourIfChangeModifyAvatar(int ticks) =>
            SexualOrgans.TickHour(ticks) | Body.TickHour(ticks);

        public event Action UpdateAvatar;
        public event Action RemoveAvatar;

        public bool HasGenderChanged()
        {
            bool didChange = lastGender != Gender;
            lastGender = Gender;
            return didChange;
        }

        public virtual void Sub()
        {
            RaceSystem.RaceChangedEvent += RaceChange;
            RaceSystem.SecRaceChangedEvent += SecRaceChange;
            DateSystem.TickMinute += TickMin;
            DateSystem.TickHour += TickHour;
        }

        public virtual void Unsub()
        {
            RaceSystem.RaceChangedEvent -= RaceChange;
            RaceSystem.SecRaceChangedEvent -= SecRaceChange;
            DateSystem.TickMinute -= TickMin;
            DateSystem.TickHour -= TickHour;
        }

        void RaceChange(BasicRace oldRace, BasicRace newRace)
        {
            if (oldRace != null)
                oldRace.RemovePrimaryRaceMods(this);
            if (newRace != null)
                newRace.AddPrimaryRaceMods(this);
        }

        void SecRaceChange(BasicRace oldRace, BasicRace newRace)
        {
            if (oldRace != null)
                oldRace.RemoveSecondaryRaceMod(this);
            if (newRace != null)
                newRace.AddSecondaryRaceMods(this);
        }


        public void Loaded()
        {
            Stats.Loaded();
            SexualOrgans.Loaded();
            Body.Loaded();
        }

        public virtual IEnumerator Load(CharacterSave toLoad)
        {
            RaceSystem = new RaceSystem();
            yield return RaceSystem.Load(toLoad.RacesSave);
            RaceChange(null, RaceSystem.Race);
            SecRaceChange(null, RaceSystem.SecRace);

            yield return LevelSystem.Load(toLoad.PerkSave);
            ReGainPerks(LevelSystem.OwnedPerks);

            yield return essenceSystem.Load(toLoad.EssPerkSave);
            ReGainPerks(Essence.EssencePerks);

            yield return vore.Load(toLoad.VorePerkSave);
            ReGainPerks(vore.Level.OwnedPerks);

            void ReGainPerks(IEnumerable<BasicPerk> perks)
            {
                foreach (BasicPerk perk in perks)
                    perk.PerkGainedEffect(this);
            }
        }

        public void GotPregnant() => SexualOrgans.Boobs.StartLactating();

        public static event Action<Child> PlayerIsTheFather;

        public virtual void OnBirth(Fetus obj)
        {
            Child newBorn = BaseOnBirth(obj);
            PlayerIsTheFather?.Invoke(newBorn);
            CharacterEvents.CharacterEvents.BirthEvent.StartEvent(this);
            InvokeUpdateAvatar();
        }

        protected Child BaseOnBirth(Fetus obj)
        {
            Child newBorn = obj.GetBorn("Child");
            FamilyTree.Children.Add(newBorn.Identity.ID);
            DayCare.AddChild(newBorn);
            InvokeUpdateAvatar();
            return newBorn;
        }

        public virtual void OnStomachDigestion(Prey prey, string digestionMode) =>
            //TODO Need balancing
            Body.Thickset.BaseValue += prey.StartWeight / 1000f;

        public virtual void OnOrganDigestionProgress(SexualOrganType organType, Prey prey, string mode, float progress)
        {
        }

        public virtual void OnOrganDigestion(SexualOrganType organType, Prey prey, string mode)
        {
        }

        public virtual void InvokeUpdateAvatar() => UpdateAvatar?.Invoke();

        public void InvokeRemoveAvatar() => RemoveAvatar?.Invoke();
    }
}