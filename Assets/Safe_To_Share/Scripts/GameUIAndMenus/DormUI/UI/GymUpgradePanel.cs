namespace DormAndHome.Dorm.UI
{
    public class GymUpgradePanel : BuildingUpgradePanel
    {
        void OnEnable() => mainBuilding.Setup(playerHolder.Player, DormManager.Instance.Buildings.Gym);
    }
}