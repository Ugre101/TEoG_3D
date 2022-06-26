namespace Character.PlayerStuff
{
    public interface IInteractable
    {
        string HoverText(Player player);
        void DoInteraction(Player player);
    }
}