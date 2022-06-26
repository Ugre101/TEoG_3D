namespace DormAndHome.Dorm.UI
{
    public class DormDungeonCanvas : DormSleepAreaShared
    {
        public override void Enter()
        {
            base.Enter();
            transform.AwakeChildren(DormManager.Instance.Buildings.Dungeon.Level > 0 ? upgradePanel : dormPanel);
        }
    }
}