namespace DormAndHome.Dorm.UI
{
    public class DormKitchenCanvas : DormBuildingCanvas
    {
        protected override bool HasBuilding => Manager.Buildings.Kitchen.Level > 0;

        protected override void OpenUpgradePanel() => base.OpenUpgradePanel();
    }
}