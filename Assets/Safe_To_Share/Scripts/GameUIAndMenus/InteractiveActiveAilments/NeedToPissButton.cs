namespace Safe_To_Share.Scripts.GameUIAndMenus.InteractiveActiveAilments
{
    public class NeedToPissButton : DoThingButton
    {
        public override void OnClick()
        {
            holder.ScatPissHandler.Piss();
        }
    }
}