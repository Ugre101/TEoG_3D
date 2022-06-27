using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Character;
using Character.CreateCharacterStuff;
using Character.EnemyStuff;
using Character.LevelStuff;
using Character.Organs.Fluids;
using Character.PlayerStuff;
using Character.PlayerStuff.Currency;
using Character.Race.Races;
using DormAndHome.Dorm;
using Movement.ECM2.Source.Characters;
using Movement.ECM2.Source.Components;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace AvatarStuff.Holders
{
    [SelectionBase]
    public class PlayerHolder : Holder
    {
        [SerializeField] MovementModHandler movementMoveModHandler;
        [SerializeField] ThirdPersonEcm2Character thirdPersonEcm2Character;

        [SerializeField] Player player;

        [SerializeField] LayerMask validLayers;

        public static PlayerHolder Instance { get; private set; }
        public static int PlayerID { get; private set; }
        public static Vector3 Position => Instance != null ? Instance.transform.position : Vector3.zero;

        public Player Player
        {
            get => player;
            private set => player = value;
        }
        public MovementModHandler MoveModHandler => movementMoveModHandler;

        public ThirdPersonEcm2Character PersonEcm2Character => thirdPersonEcm2Character;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Debug.LogError("Duplicate player holders");
                Destroy(gameObject);
            }
        }

        void OnDisable() => UnSub();
        void OnDestroy() => UnSub();
        public event Action RePlaced;

        protected override void Sub()
        {
            base.Sub();
            // UpdateAvatar(Player);
            Player.RaceSystem.RaceChangedEvent += RaceChange;
            Player.Body.Height.StatDirtyEvent += UpdateHeightsChange;
            player.Body.Muscle.StatDirtyEvent += ModifyCurrentAvatar;
            player.Body.Fat.StatDirtyEvent += ModifyCurrentAvatar;
            HeightsChange(Player.Body.Height.Value);
            Player.Sub();
            PersonEcm2Character.PhysicsVolumeChanged += IsSwimming;
        }

        void IsSwimming(PhysicsVolume newvolume)
        {
            if (newvolume == null || !newvolume.waterVolume) return;
            ForeignFluidExtensions.CleanBody(player);
        }

        public void UpdateAvatar() => UpdateAvatar(Player);

        protected override void UnSub()
        {
            base.UnSub();
            if (Player == null)
                return;
            if (Player.RaceSystem != null)
                Player.RaceSystem.RaceChangedEvent -= RaceChange;
            Player.UpdateAvatar -= ModifyCurrentAvatar;
            Player.Body.Height.StatDirtyEvent -= UpdateHeightsChange;
            player.Body.Muscle.StatDirtyEvent -= ModifyCurrentAvatar;
            player.Body.Fat.StatDirtyEvent -= ModifyCurrentAvatar;
            PersonEcm2Character.PhysicsVolumeChanged -= IsSwimming;
            Player.Unsub();
        }

        void UpdateHeightsChange() => HeightsChange(Player.Body.Height.Value);


        protected override void NewAvatar(CharacterAvatar obj)
        {
            if (CurrentAvatar != null)
                Player.UpdateAvatar -= ModifyCurrentAvatar;
            CurrentAvatar = obj;
            CurrentAvatar.Setup(Player);
            Player.UpdateAvatar += ModifyCurrentAvatar;
        }

        public void ModifyCurrentAvatar()
        {
            if (CurrentAvatar == null) return;
            CurrentAvatar.Setup(Player);
            HeightsChange(player.Body.Height.Value);
        }

        void RaceChange(BasicRace oldrace, BasicRace newrace) => UpdateAvatar(Player);

        public IEnumerator Load(PlayerSave toLoad)
        {
            UnSub();
            Player = JsonUtility.FromJson<Player>(toLoad.ControlledCharacterSave.CharacterSave.RawCharacter);
            PersonEcm2Character.StopSwimming();
            SetPlayerPosition(toLoad.Posistion);

            yield return Player.Load(toLoad.ControlledCharacterSave.CharacterSave);
            AddMovementMods();

            Player.Inventory.Load(toLoad.InventorySave);
            Player.AndSpellBook.Load(toLoad.ControlledCharacterSave.AbilitySave);
            var wait = UpdateAvatar(Player);
            while (!wait.IsCompleted)
            {
                yield return null;
            }
            if (wait.IsFaulted)
                throw wait.Exception;
            NewPlayer();
        }

        void SetPlayerPosition(Vector3 toLoad)
        {
            if (RayCast(1f, 10f))
                return;
            if (RayCast(10f, 100f))
                return;
            SetPlayerToDefaultPos();

            bool RayCast(float yOffset, float distance)
            {
                if (!Physics.Raycast(new Ray(toLoad + new Vector3(0, yOffset, 0), Vector3.down),
                        out RaycastHit hit, distance, validLayers))
                    return false;
                transform.position = hit.point + new Vector3(0, 2, 0);
                return true;
            }
        }

        void SetPlayerToDefaultPos()
        {
            Vector3 terrainDataSize = Terrain.activeTerrain.terrainData.size;
            Vector3 terrainPos = Terrain.activeTerrain.GetPosition();
            Vector3 center = new(terrainPos.x + terrainDataSize.x / 2, 100f, terrainPos.z + terrainDataSize.z / 2);
            transform.position = center;
        }

        void AddMovementMods()
        {
            MoveModHandler.Reset();
            foreach (BasicPerk perk in Player.LevelSystem.OwnedPerks.Where(op => op.PerkType == PerkType.Movement))
                if (perk is MovementPerk movementPerk)
                    movementPerk.GainMovementMods(this);
        }

        public async Task ReplacePlayer(Player newPlayer)
        {
            UnSub();
            Player = newPlayer;
            AddMovementMods();
            Player.Vore.Level.LoadMyPerkAssets();
            Player.Essence.LoadMyPerkAssets();
            Stopwatch sw = new();
            sw.Start();
            await UpdateAvatar(newPlayer);
            sw.Stop();
            print("load time " + sw.Elapsed);
            NewPlayer();
        }

        void NewPlayer()
        {
            Player.Loaded();
            Sub();
            PlayerID = Player.Identity.ID;
            RePlaced?.Invoke();
        }

        /*
         bool loadingBattle;

        public void TriggerCombat(params BaseCharacter[] with)
        {
            if (loadingBattle)
                return;
            loadingBattle = true;
            SceneLoader.Instance.LoadCombat(this, with);
        }

        public void TriggerSex(params BaseCharacter[] with)
        {
            if (loadingBattle)
                return;
            loadingBattle = true;
            SceneLoader.Instance.LoadAfterBattle(this, with);
        }
        */
#if UNITY_EDITOR
        [SerializeField] CharacterPreset playerChar;
        public async Task EditorSetup() => await LoadPresetThenReplace();

        async Task LoadPresetThenReplace()
        {
            await playerChar.LoadAssets();
            await ReplacePlayer(new Player(playerChar.NewCharacter()));
            player.LevelSystem.GainExp(999);
            Player.Vore.Level.GainExp(999);
            PlayerGold.GoldBag.GainGold(9999);
        }
#endif
        public void TriggerCombat(BaseCharacter[] enemy)
        {
            LoadCombat?.Invoke(Player, enemy);
        }

        public static Action<Player, BaseCharacter[]> LoadCombat;
        public static Action<PlayerHolder, DormMate> LoadDormSex;

        public void TriggerSex(DormMate mate)
        {
            LoadDormSex?.Invoke(this,mate);
        }
    }
}