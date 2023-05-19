using Safe_To_Share.Scripts.Static;

namespace Safe_To_Share.Scripts.GameUIAndMenus.DormUI.UI
{
    public sealed class DormCanvas : DormSleepAreaShared
    {
        public override void Enter()
        {
            base.Enter();
            transform.AwakeChildren(upgradePanel);
        }
    }
}