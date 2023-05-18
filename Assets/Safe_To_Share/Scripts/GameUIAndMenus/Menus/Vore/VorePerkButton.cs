using System;
using Character.LevelStuff;
using Character.VoreStuff;
using Safe_To_Share.Scripts.GameUIAndMenus.Menus.Level;
using Safe_To_Share.Scripts.Holders;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Safe_To_Share.Scripts.GameUIAndMenus.Menus.Vore
{
    public class VorePerkButton : BasePerkButton, IPointerEnterHandler
    {
        public void OnPointerEnter(PointerEventData eventData) => ShowPerkInfo?.Invoke(transform.position, loaded);

        //  new VorePerk loaded;
        public override void Setup(PlayerHolder player)
        {
            this.PlayerHolder = player;
            Addressables.LoadAssetAsync<VorePerk>(guid).Completed += Loaded;
        }

        public static event Action<Vector3, BasicPerk> ShowPerkInfo;

        void HasPerk(VorePerk obj)
        {
            bool havePerk = Player.Vore.Level.OwnedPerks.Contains(loaded as VorePerk);
            HaveFade(havePerk);
            CanAfford(loaded.Cost);
        }

        void Loaded(AsyncOperationHandle<VorePerk> obj)
        {
            loaded = obj.Result;
            CanAfford(Player.Vore.Level.Points);
            HasPerk(obj.Result);
        }

        protected override void CanAfford(int obj) => Afford = loaded.Cost <= obj;

        protected override void OnClick()
        {
            if (Have || !loaded.MeetsRequirements(Player) || !Player.Vore.Level.TryUsePoints(loaded.Cost))
                return;
            if (loaded is not VorePerk perk)
                return;
            perk.GainPerk(Player);
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