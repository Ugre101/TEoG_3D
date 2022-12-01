using System;
using AvatarStuff.Holders;
using Character.LevelStuff;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameUIAndMenus.Menus.Level
{
    public class PerkButton : BasePerkButton, IPointerEnterHandler
    {
        public void OnPointerEnter(PointerEventData eventData) => ShowPerkInfo?.Invoke(transform.position, loaded);
        public static event Action<Vector3, BasicPerk> ShowPerkInfo;

        protected override void OnClick()
        {
            if (Have || !loaded.MeetsRequirements(PlayerHolder.Player) ||
                !PlayerHolder.Player.LevelSystem.TryUsePoints(loaded.Cost))
                return;
            loaded.GainPerk(PlayerHolder);
            HaveFade(true);
            //GainPerk?.Invoke(loaded.Cost, loaded);
        }
    }
}