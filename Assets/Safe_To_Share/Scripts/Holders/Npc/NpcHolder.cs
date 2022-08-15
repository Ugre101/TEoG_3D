using System;
using Character.PlayerStuff;
using UnityEngine;

namespace AvatarStuff.Holders.Npc
{
    public abstract class NpcHolder : MonoBehaviour, IInteractable
    {
        public abstract string HoverText(Player player);
        public abstract void DoInteraction(Player player);
        public event Action<IInteractable> UpdateHoverText;
        public event Action RemoveIInteractableHit;
    }
}