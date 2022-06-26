namespace DormAndHome.Dorm.UI
{
    public class DormCanvas : DormSleepAreaShared
    {
        public override void Enter()
        {
            base.Enter();
            transform.AwakeChildren(upgradePanel);
        }
    }
}