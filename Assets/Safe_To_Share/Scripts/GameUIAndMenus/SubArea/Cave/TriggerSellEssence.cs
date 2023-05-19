using System;
using Character.PlayerStuff;
using UnityEngine;

namespace Safe_To_Share.Scripts.GameUIAndMenus.SubArea.Cave
{
    public sealed class TriggerSellEssence : MonoBehaviour, IInteractable
    {
        [SerializeField] SellEssenceMenu menu;

        public void DoInteraction(Player player) => menu.Setup(player);
        public event Action<IInteractable> UpdateHoverText;
        public event Action RemoveIInteractableHit;

        public string HoverText(Player player) => "Sell Essence";
    }
}