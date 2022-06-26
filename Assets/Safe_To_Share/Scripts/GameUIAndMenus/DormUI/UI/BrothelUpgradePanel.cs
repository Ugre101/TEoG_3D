namespace DormAndHome.Dorm.UI
{
    public class BrothelUpgradePanel : BuildingUpgradePanel
    {
        void OnEnable() =>
            mainBuilding.Setup(playerHolder.Player, DormManager.Instance.Buildings.VillageBuildings.Brothel);
    }
}