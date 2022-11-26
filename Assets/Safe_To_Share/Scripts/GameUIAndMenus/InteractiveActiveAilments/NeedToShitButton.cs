namespace Safe_To_Share.Scripts.GameUIAndMenus.InteractiveActiveAilments
{
    public class NeedToShitButton : DoThingButton
    {
        public override void OnClick()
        {
            holder.StartShitting();
            gameObject.SetActive(false);
        }

    }
}