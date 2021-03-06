using System;
using AvatarStuff.Holders;
using Character.EssenceStuff;
using Character.LevelStuff;
using Character.PlayerStuff;
using GameUIAndMenus.Menus.Level;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameUIAndMenus.Menus.EssenceMenu
{
    public class EssencePerkButton : BasePerkButton, IPointerEnterHandler
    {
        public void OnPointerEnter(PointerEventData eventData) => ShowPerkInfo?.Invoke(transform.position, loaded);
        public static event Action<Vector3, BasicPerk> ShowPerkInfo;

//        new EssencePerk loaded;
        public override void Setup(PlayerHolder player)
        {
            this.player = player;
            Addressables.LoadAssetAsync<EssencePerk>(guid).Completed += LoadedPerk;
        }

        private void LoadedPerk(AsyncOperationHandle<EssencePerk> obj)
        {
            loaded = obj.Result;
            CanAfford(player.Player.LevelSystem.Points);
            HasPerk(obj.Result);
        }

        protected void HasPerk(EssencePerk result)
        {
            bool hasPerk = player.Player.Essence.EssencePerks.Contains(result);
            HaveFade(hasPerk);
        }
        protected override void OnClick()
        {
            if (Have || !loaded.MeetsRequirements(player.Player) || !player.Player.LevelSystem.TryUsePoints(loaded.Cost))
                return;
            if (loaded is not EssencePerk perk)
                return;
            HaveFade(true);
            EssenceExtensions.GainPerk(perk, player.Player);
        }
    }
}