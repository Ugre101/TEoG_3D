using System;
using AvatarStuff.Holders;
using Character.LevelStuff;
using Character.PlayerStuff;
using Character.VoreStuff;
using GameUIAndMenus.Menus.Level;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameUIAndMenus.Menus.Vore
{
    public class VorePerkButton : BasePerkButton, IPointerEnterHandler
    {
      //  new VorePerk loaded;
        public override void Setup(PlayerHolder player)
        {
            this.player = player;
            Addressables.LoadAssetAsync<VorePerk>(guid).Completed += Loaded;
        }

        public void OnPointerEnter(PointerEventData eventData) => ShowPerkInfo?.Invoke(transform.position, loaded);

        public static event Action<Vector3, BasicPerk> ShowPerkInfo;
        void HasPerk(VorePerk obj)
        {
            bool havePerk = player.Player.Vore.Level.OwnedPerks.Contains(loaded as VorePerk);
            HaveFade(havePerk);
            CanAfford(loaded.Cost);
        }

        void Loaded(AsyncOperationHandle<VorePerk> obj)
        {
            loaded = obj.Result;
            CanAfford(player.Player.Vore.Level.Points);
            HasPerk(obj.Result);
        }

        protected override void CanAfford(int obj) => Afford = loaded.Cost <= obj;

        protected override void OnClick()
        {
            if (Have || !loaded.MeetsRequirements(player.Player) || !player.Player.Vore.Level.TryUsePoints(loaded.Cost))
                return;
            if (loaded is not VorePerk perk)
                return;
            perk.GainPerk(player.Player);
            HaveFade(true);
        }

#if UNITY_EDITOR
        [SerializeField] VorePerk vorePerk;
        void OnValidate()
        {
            guid = vorePerk != null ? vorePerk.Guid : string.Empty;
            if (rune == null || vorePerk == null)
                return;
            rune.sprite = vorePerk.Icon;
            btnText.text = vorePerk.Title;
        }
#endif
    }
}