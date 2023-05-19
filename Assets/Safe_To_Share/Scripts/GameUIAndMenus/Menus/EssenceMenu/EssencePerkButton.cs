using System;
using Character.EssenceStuff;
using Character.LevelStuff;
using Safe_To_Share.Scripts.GameUIAndMenus.Menus.Level;
using Safe_To_Share.Scripts.Holders;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Safe_To_Share.Scripts.GameUIAndMenus.Menus.EssenceMenu
{
    public sealed class EssencePerkButton : BasePerkButton, IPointerEnterHandler
    {
        public void OnPointerEnter(PointerEventData eventData) => ShowPerkInfo?.Invoke(transform.position, loaded);
        public static event Action<Vector3, BasicPerk> ShowPerkInfo;

//        new EssencePerk loaded;
        public override void Setup(PlayerHolder player)
        {
            PlayerHolder = player;
            Addressables.LoadAssetAsync<EssencePerk>(guid).Completed += LoadedPerk;
        }

        void LoadedPerk(AsyncOperationHandle<EssencePerk> obj)
        {
            loaded = obj.Result;
            CanAfford(Player.LevelSystem.Points);
            HasPerk(obj.Result);
        }

        void HasPerk(EssencePerk result)
        {
            bool hasPerk = Player.Essence.EssencePerks.Contains(result);
            HaveFade(hasPerk);
        }

        protected override void OnClick()
        {
            if (Have || !loaded.MeetsRequirements(Player) ||
                !Player.LevelSystem.TryUsePoints(loaded.Cost))
                return;
            if (loaded is not EssencePerk perk)
                return;
            HaveFade(true);
            perk.GainPerk(Player);
        }
    }
}