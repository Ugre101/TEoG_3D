using System;
using Character.PlayerStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.IslandDataUI
{
    public class IslandStone : MonoBehaviour, IInteractable
    {
        [SerializeField] IslandStoneCanvas stoneCanvas;
        public void DoInteraction(Player player) => stoneCanvas.Open(player); //OpenStoneMenu?.Invoke();
        public event Action<IInteractable> UpdateHoverText;
        public event Action RemoveIInteractableHit;
        public string HoverText(Player player) => "Island stone";
    }
}