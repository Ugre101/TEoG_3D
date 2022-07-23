using System.Collections.Generic;
using System.Linq;
using Character;
using Character.EnemyStuff;
using Character.EssenceStuff;
using Character.PlayerStuff;
using DormAndHome.Dorm;
using Safe_To_Share.Scripts.AfterBattle.UI;
using Safe_To_Share.Scripts.Static;
using SceneStuff;
using UnityEngine.AddressableAssets;

namespace Safe_To_Share.Scripts.AfterBattle
{
    public class AfterBattleMain : AfterBattleShared
    {
        static AfterBattleMainUI AfterBattleMainUI => AfterBattleMainUI.Instance;

        void OnDestroy()
        {
            SexActionButton.PlayerAction -= HandlePlayerAction;
            DrainActionButton.PlayerAction -= HandleDrainAction;
            VoreActionButton.PlayerAction -= HandleVoreAction;
            TakeToDormButton.TakeToDorm -= HandleTakeToDorm;
        }

        void HandleTakeToDorm()
        {
            if (activeEnemyActor.Actor is not Enemy enemy || !enemy.CanTakeHome() || !DormManager.Instance.DormHasSpace)
                return;
            DormManager.Instance.AddToDorm(enemy);
            activeEnemyActor.Removed();
            AfterBattleMainUI.NoPartnerRefresh(activePlayerActor.Actor);
            enemy.GotRemoved();
        }


        void BasePlayerActionReaction(AfterBattleBaseAction obj)
        {
            SexActData data = obj.Use(activePlayerActor, activeEnemyActor);
            activePlayerActor.SetActAnimation(data.Giver);
            activeEnemyActor.SetActAnimation(data.Receiver);
            AfterBattleMainUI.LogText(data);
        }


        void HandlePlayerAction(AfterBattleBaseAction obj)
        {
            BasePlayerActionReaction(obj);
            AfterBattleMainUI.RefreshButtons(activePlayerActor.Actor, activeEnemyActor.Actor);
        }


        void HandleDrainAction(AfterBattleBaseAction action)
        {
            BasePlayerActionReaction(action);
            activePlayerActor.ModifyAvatar();
            activeEnemyActor.ModifyAvatar();
            AfterBattleMainUI.RefreshButtons(activePlayerActor.Actor, activeEnemyActor.Actor);
        }

        void HandleVoreAction(AfterBattleBaseAction obj)
        {
            BasePlayerActionReaction(obj);
            activePlayerActor.ModifyAvatar();
            activeEnemyActor.Removed();
            AfterBattleMainUI.NoPartnerRefresh(activePlayerActor.Actor);
            if (activeEnemyActor.Actor is not Enemy enemy)
                return;
            enemy.GotRemoved();
        }

        public override void Setup(Player player, BaseCharacter[] enemies, params BaseCharacter[] allies)
        {
            SexActionButton.PlayerAction += HandlePlayerAction;
            DrainActionButton.PlayerAction += HandleDrainAction;
            VoreActionButton.PlayerAction += HandleVoreAction;
            TakeToDormButton.TakeToDorm += HandleTakeToDorm;
            player.SexStats.NewSession();
            transform.AwakeChildren();
            SetPlayerActor(player);
            if (allies != null)
            {
                // TODO
            }

            SetEnemyActor(enemies[0]);
            AfterBattleMainUI.Setup(player, activePlayerActor.Actor, activeEnemyActor.Actor);
            SceneLoader.Instance.PreLoadDepLast();
        }


        void SetPlayerActor(BaseCharacter character)
        {
            activePlayerActor.Setup(character);
            LoadEssencePerks(character.LevelSystem.OwnedPerks.OfType<EssencePerk>());
            AfterBattleMainUI.SetupPlayer(character);
        }

        static void LoadEssencePerks(IEnumerable<EssencePerk> enumerable)
        {
            if (enumerable == null)
                return;
            foreach (var perk in enumerable)
                Addressables.LoadAssetAsync<EssencePerk>(perk.Guid);
        }

        void SetEnemyActor(BaseCharacter character)
        {
            activeEnemyActor.Setup(character);
            LoadEssencePerks(character.LevelSystem.OwnedPerks.OfType<EssencePerk>());
            AfterBattleMainUI.SetupPartner(character);
        }
    }
}