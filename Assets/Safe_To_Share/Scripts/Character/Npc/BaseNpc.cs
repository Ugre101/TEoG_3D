using Character.PlayerStuff;

namespace Character.Npc
{
    [System.Serializable]
    public abstract class BaseNpc : BaseCharacter, IInteractable
    {
        public abstract string HoverText(Player player);
        public abstract void DoInteraction(Player player);
    }
}