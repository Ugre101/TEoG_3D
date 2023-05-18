namespace Safe_To_Share.Scripts.GameUIAndMenus.DormUI.UI
{
    public class DormGymCanvas : DormBuildingCanvas
    {
        protected override bool HasBuilding => Manager.Buildings.Gym.Level > 0;

        protected override void OpenUpgradePanel() => base.OpenUpgradePanel();
    }
}