using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using AvatarStuff;
using Character;
using Character.CreateCharacterStuff;
using Character.LevelStuff;
using Character.Organs.Fluids;
using Character.PlayerStuff;
using Character.PlayerStuff.Currency;
using Character.Race.Races;
using DormAndHome.Dorm;
using Safe_To_Share.Scripts.AvatarStuff.ScatAndPiss;
using Safe_To_Share.Scripts.Character.Scat;
using Safe_To_Share.Scripts.Movement.HoverMovement;
using Safe_To_Share.Scripts.Static;
using UnityEngine;

namespace Safe_To_Share.Scripts.Holders {
    [SelectionBase]
    public sealed class PlayerHolder : Holder {
        public delegate void CombatParameters(Player player, params BaseCharacter[] enemies);

        public delegate void SubRealmCombatParameters(Player player, bool exitToLastLocationOnDefeat,
                                                      params BaseCharacter[] enemies);

        public static CombatParameters LoadCombat;
        public static SubRealmCombatParameters LoadSubRealmCombat;
        public static Action<PlayerHolder, DormMate> LoadDormSex;
        [SerializeField] MovementModHandler movementMoveModHandler;
        [SerializeField] Movement.HoverMovement.Movement mover;
        [SerializeField] Player player;

        [SerializeField] LayerMask validLayers;
        AvatarScatPissManager avatarScatPissHandler;

        bool combat;

        public static PlayerHolder Instance { get; private set; }
        public static int PlayerID { get; private set; }
        public static Vector3 Position => Instance != null ? Instance.transform.position : Vector3.zero;

        public Player Player => player;

        public MovementModHandler MoveModHandler => movementMoveModHandler;

        public Movement.HoverMovement.Movement PersonEcm2Character => mover;

        void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Debug.LogError("Duplicate player holders");
                Destroy(gameObject);
            }
        }

        void Start() {
            transform.SetParent(null);
        }

        void OnDisable() => UnSub();

        void OnDestroy() {
            UnSub();
#if UNITY_EDITOR
            playerChar.UnLoad();
#endif
        }

        void OnCollisionEnter(Collision collision) {
            // if (collision.gameObject.CompareTag("Enemy"))
            // {
            //     if (collision.gameObject.TryGetComponent(out EnemyAiHolder enemy)) 
            //         TriggerCombat(enemy.Enemy);
            // }
        }

        public event Action RePlaced;

        protected override void Sub() {
            base.Sub();
            // UpdateAvatar(Player);
            player.RaceSystem.RaceChangedEvent += RaceChange;
            player.Body.Height.StatDirtyEvent += UpdateHeightsChange;
            player.Body.Muscle.StatDirtyEvent += ModifyCurrentAvatar;
            player.Body.Fat.StatDirtyEvent += ModifyCurrentAvatar;
            HeightsChange(player.Body.Height.Value);
            player.Sub();
            PersonEcm2Character.ChangedMode += IsSwimming;
        }

        void IsSwimming(MoveCharacter.MoveModes moveModes) {
            if (moveModes != MoveCharacter.MoveModes.Swimming)
                return;
            ForeignFluidExtensions.CleanBody(player);
        }

        public async void UpdateAvatar() => await UpdateAvatar(player);

        protected override void UnSub() {
            base.UnSub();
            if (player == null)
                return;
            if (player.RaceSystem != null)
                player.RaceSystem.RaceChangedEvent -= RaceChange;
            player.UpdateAvatar -= ModifyCurrentAvatar;
            player.Body.Height.StatDirtyEvent -= UpdateHeightsChange;
            player.Body.Muscle.StatDirtyEvent -= ModifyCurrentAvatar;
            player.Body.Fat.StatDirtyEvent -= ModifyCurrentAvatar;
            PersonEcm2Character.ChangedMode -= IsSwimming;
            player.Unsub();
        }

        void UpdateHeightsChange() => HeightsChange(player.Body.Height.Value);


        protected override void NewAvatar(CharacterAvatar obj) {
            player.UpdateAvatar -= ModifyCurrentAvatar;
            Changer.CurrentAvatar.Setup(player);
            player.UpdateAvatar += ModifyCurrentAvatar;
            if (obj.TryGetComponent(out AvatarScatPissManager pissManager))
                avatarScatPissHandler = pissManager;
            ModifyCurrentAvatar();
        }

        public void ModifyCurrentAvatar() {
            if (Changer.CurrentAvatar == null) return;
            Changer.CurrentAvatar.Setup(player);
            HeightsChange(player.Body.Height.Value);
        }

        async void RaceChange(BasicRace oldrace, BasicRace newrace) => await UpdateAvatar(player);

        public IEnumerator Load(PlayerSave toLoad) {
            UnSub();
            player = JsonUtility.FromJson<Player>(toLoad.ControlledCharacterSave.CharacterSave.RawCharacter);
            //PersonEcm2Character.StopSwimming();
            SetPlayerPosition(toLoad.Posistion);

            yield return player.Load(toLoad.ControlledCharacterSave.CharacterSave);
            AddMovementMods();

            player.Inventory.Load(toLoad.InventorySave);
            player.AndSpellBook.Load(toLoad.ControlledCharacterSave.AbilitySave);
            var wait = UpdateAvatar(player);
            while (!wait.IsCompleted) yield return null;
            NewAvatar(Changer.CurrentAvatar);
            if (wait is { IsFaulted: true, Exception: not null, }) throw wait.Exception;
            NewPlayer();
        }

        void SetPlayerPosition(Vector3 toLoad) {
            if (RayCast(1f, 10f))
                return;
            if (RayCast(10f, 100f))
                return;
            SetPlayerToDefaultPos();

            bool RayCast(float yOffset, float distance) {
                if (!Physics.Raycast(new Ray(toLoad + new Vector3(0, yOffset, 0), Vector3.down),
                        out var hit, distance, validLayers))
                    return false;
                transform.position = hit.point + new Vector3(0, 2, 0);
                return true;
            }
        }

        void SetPlayerToDefaultPos() {
            var terrainDataSize = Terrain.activeTerrain.terrainData.size;
            var terrainPos = Terrain.activeTerrain.GetPosition();
            Vector3 center = new(terrainPos.x + terrainDataSize.x / 2, 100f, terrainPos.z + terrainDataSize.z / 2);
            transform.position = center;
        }

        void AddMovementMods() {
            MoveModHandler.Reset();
            foreach (var perk in player.LevelSystem.OwnedPerks.Where(op => op.PerkType == PerkType.Movement))
                if (perk is MovementPerk movementPerk)
                    movementPerk.GainMovementMods(this);
        }

        public async Task ReplacePlayer(Player newPlayer) {
            UnSub();
            player = newPlayer;
            AddMovementMods();
            player.Vore.Level.LoadMyPerkAssets();
            player.Essence.LoadMyPerkAssets();
            await UpdateAvatar(newPlayer);
            NewAvatar(Changer.CurrentAvatar);
            NewPlayer();
        }

        void NewPlayer() {
            player.Loaded();
            Sub();
            PlayerID = player.Identity.ID;
            RePlaced?.Invoke();
        }

        public void TriggerCombat(params BaseCharacter[] enemy) {
            if (combat) return;
            combat = true;
            LoadCombat?.Invoke(player, enemy);
        }

        public void TriggerSubRealmCombat(BaseCharacter[] enemy, bool kickOutOnDefeat) {
            if (combat) return;
            combat = true;
            LoadSubRealmCombat?.Invoke(player, kickOutOnDefeat, enemy);
        }

        public void TriggerSex(DormMate mate) => LoadDormSex?.Invoke(this, mate);

        public void StartPissing() {
            if (OptionalContent.Scat.Enabled is false) return;
            if (player.BodyFunctions.Bladder.Pressure() < ScatExtensions.NeedToPissThreesHold)
                return;
            avatarScatPissHandler.Piss(player.BodyFunctions.Bladder.Empty(), avatarScaler.Height);
        }

        public void StartShitting() {
            if (OptionalContent.Scat.Enabled is false) return;
            if (player.SexualOrgans.Anals.Fluid.CurrentValue / player.SexualOrgans.Anals.Fluid.Value <
                ScatExtensions.NeedToShitThreesHold)
                return;
            avatarScatPissHandler.Scat(avatarScaler.Height);
            player.SexualOrgans.Anals.Fluid.SetEmpty();
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
        [Header("Debug stuff"), SerializeField,]
        CharacterPreset playerChar;

        public async Task EditorSetup() => await LoadPresetThenReplace();

        async Task LoadPresetThenReplace() {
            await playerChar.LoadAssets();
            await ReplacePlayer(new Player(playerChar.NewCharacter()));
            player.LevelSystem.GainExp(999);
            player.Vore.Level.GainExp(999);
            PlayerGold.GoldBag.GainGold(9999);
        }

        [SerializeField] int fetusDaysOld = 279;

        [ContextMenu("Give fetus")]
        public void AddDebugFetus() {
            foreach (var baseOrgan in player.SexualOrgans.Vaginas.BaseList) {
                baseOrgan.Womb.AddFetus(player, player);
                baseOrgan.Womb.GrowFetuses(fetusDaysOld);
                break;
            }
        }
#endif
    }
}