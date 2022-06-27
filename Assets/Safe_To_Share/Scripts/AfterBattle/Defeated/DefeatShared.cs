using System;
using Character;
using Character.PlayerStuff;
using Safe_To_Share.Scripts.AfterBattle.Defeated.UI;
using Safe_To_Share.Scripts.Static;

namespace Safe_To_Share.Scripts.AfterBattle.Defeated
{
    public abstract class DefeatShared : AfterBattleShared
    {
        protected static readonly Random Rng = new();

        protected int resistance;

        protected static DefeatMainUI UI => DefeatMainUI.Instance;

        public int Resistance
        {
            get => resistance;
            set
            {
                resistance = value;
                ResistanceChange?.Invoke(value);
            }
        }

        void OnDestroy()
        {
            DefeatMainUI.Resist -= HandleResist;
            DefeatMainUI.GiveIn -= HandleGiveIn;
            DefeatMainUI.Continue -= HandleContinue;
        }

        public static event Action<int> ResistanceChange;
        protected abstract void HandleGiveIn();
        protected abstract void HandleResist();
        protected abstract void HandleContinue();

        protected void SharedSetup(Player player, BaseCharacter[] enemies, params BaseCharacter[] allies)
        {
            DefeatMainUI.Resist += HandleResist;
            DefeatMainUI.GiveIn += HandleGiveIn;
            DefeatMainUI.Continue += HandleContinue;
            Resistance = 100;
            transform.AwakeChildren();
            player.SexStats.NewSession();
            SetupPlayer(player);
            SetupEnemy(enemies[0]);
            UI.Setup(player);
        }

        void SetupEnemy(BaseCharacter enemies)
        {
            activeEnemyActor.Setup(enemies);
            UI.SetupPartner(enemies);
        }

        void SetupPlayer(BaseCharacter player)
        {
            activePlayerActor.Setup(player);
            UI.SetupPlayer(player);
        }

        protected static void Leave() => UI.Leave();
    }
}