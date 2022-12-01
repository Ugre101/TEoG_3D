using Safe_To_Share.Scripts.Character.Scat;
using Safe_To_Share.Scripts.Static;

namespace Safe_To_Share.Scripts.GameUIAndMenus.InteractiveActiveAilments
{
    public class NeedToPissButton : DoThingButton
    {
        protected override float ThreesHold => ScatExtensions.NeedToPissThreesHold;
        protected override bool Enabled => OptionalContent.Scat.Enabled;

        public override void OnClick()
        {
            holder.StartPissing();
            gameObject.SetActive(false);
        }
    }
}