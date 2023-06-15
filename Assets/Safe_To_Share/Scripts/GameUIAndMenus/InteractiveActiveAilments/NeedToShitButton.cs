using Safe_To_Share.Scripts.Character.Scat;
using Safe_To_Share.Scripts.Static;

namespace Safe_To_Share.Scripts.GameUIAndMenus.InteractiveActiveAilments {
    public sealed class NeedToShitButton : DoThingButton {
        protected override float ThreesHold => ScatExtensions.NeedToShitThreesHold;
        protected override bool Enabled => OptionalContent.Scat.Enabled;

        public override void OnClick() {
            holder.StartShitting();
            gameObject.SetActive(false);
        }
    }
}