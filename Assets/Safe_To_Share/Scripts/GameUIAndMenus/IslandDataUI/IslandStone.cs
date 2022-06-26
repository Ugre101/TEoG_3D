using System;
using Character.PlayerStuff;
using UnityEngine;

namespace GameUIAndMenus.IslandDataUI
{
    public class IslandStone : MonoBehaviour, IInteractable
    {
        public void DoInteraction(Player player) => OpenStoneMenu?.Invoke();
        public string HoverText(Player player) => "Island stone";
        public event Action OpenStoneMenu;
    }
}