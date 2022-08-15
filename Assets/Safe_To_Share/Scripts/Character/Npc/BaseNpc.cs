using System;
using Character.PlayerStuff;

namespace Character.Npc
{
    [Serializable]
    public abstract class BaseNpc : BaseCharacter, IInteractable
    {
        public abstract string HoverText(Player player);
        public abstract void DoInteraction(Player player);
        public event Action<IInteractable> UpdateHoverText;
        public event Action RemoveIInteractableHit;
    }
}