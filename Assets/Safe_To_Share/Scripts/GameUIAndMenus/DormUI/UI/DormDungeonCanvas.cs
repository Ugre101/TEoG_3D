using DormAndHome.Dorm;
using Safe_To_Share.Scripts.Static;

namespace Safe_To_Share.Scripts.GameUIAndMenus.DormUI.UI {
    public sealed class DormDungeonCanvas : DormSleepAreaShared {
        public override void Enter() {
            base.Enter();
            transform.AwakeChildren(DormManager.Instance.Buildings.Dungeon.Level > 0 ? upgradePanel : dormPanel);
        }
    }
}