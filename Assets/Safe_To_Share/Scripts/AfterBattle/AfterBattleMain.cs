using System.Collections;
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
using UnityEngine;
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


        IEnumerator BasePlayerActionReaction(AfterBattleBaseAction obj)
        {
            var data = obj.Use(activePlayerActor, activeEnemyActor);
            AfterBattleMainUI.LogText(data);
            if (LastAct != null && LastAct == obj)
                yield break;
            activePlayerActor.RotateActor.ResetPosAndRot();
            activeEnemyActor.RotateActor.ResetPosAndRot();
            var ani = data.SexAnimationFilterList.GetAnimation(activePlayerActor, activeEnemyActor);
            // TODO Grounding
            actorPositionManager.PosActors(activePlayerActor, ani.GivePos, activeEnemyActor,
                ani.ReceivePos, ani.StayGrounded);
            activePlayerActor.SetActAnimation(ani.GiveAnimationHash);
            activeEnemyActor.SetActAnimation(ani.ReceiveAnimationHash);
            LastAct = obj;
            if (ani.Delay > 0)
                yield return new WaitForSeconds(ani.Delay);
        }


        void HandlePlayerAction(AfterBattleBaseAction obj)
        {
            StartCoroutine(PlayerActionReaction());

            IEnumerator PlayerActionReaction()
            {
                yield return BasePlayerActionReaction(obj);
                AfterBattleMainUI.RefreshButtons(activePlayerActor.Actor, activeEnemyActor.Actor);
            }
        }


        void HandleDrainAction(AfterBattleBaseAction action)
        {
            StartCoroutine(DrainActionReaction());

            IEnumerator DrainActionReaction()
            {
                yield return BasePlayerActionReaction(action);
                activePlayerActor.ModifyAvatar();
                activeEnemyActor.ModifyAvatar();
                AfterBattleMainUI.RefreshButtons(activePlayerActor.Actor, activeEnemyActor.Actor);
            }
        }


        void HandleVoreAction(AfterBattleBaseAction obj)
        {
            StartCoroutine(HandleVoreReaction());

            IEnumerator HandleVoreReaction()
            {
                yield return BasePlayerActionReaction(obj);
                activePlayerActor.ModifyAvatar();
                activeEnemyActor.Removed();
                AfterBattleMainUI.NoPartnerRefresh(activePlayerActor.Actor);
                if (activeEnemyActor.Actor is not Enemy enemy)
                    yield break;
                enemy.GotRemoved();
                activePlayerActor.RotateActor.ResetPosAndRot();
                activePlayerActor.AlignActor.Stop();
            }
        }

        public override void Setup(Player player, BaseCharacter[] enemies, params BaseCharacter[] allies)
        {
            SexActionButton.PlayerAction += HandlePlayerAction;
            DrainActionButton.PlayerAction += HandleDrainAction;
            VoreActionButton.PlayerAction += HandleVoreAction;
            TakeToDormButton.TakeToDorm += HandleTakeToDorm;
            player.SexStats.NewSession();
            transform.AwakeChildren();
            activePlayerActor.AvatarScaler.SetHeight(player.Body.Height.Value,true);
            activeEnemyActor.AvatarScaler.SetHeight(enemies[0].Body.Height.Value,false);
            
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
            activePlayerActor.Setup(character, animatorController);
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
            activeEnemyActor.Setup(character, animatorController);
            LoadEssencePerks(character.LevelSystem.OwnedPerks.OfType<EssencePerk>());
            AfterBattleMainUI.SetupPartner(character);
        }
    }
}