namespace DormAndHome.Dorm.UI
{
    public class KitchenUpgradePanel : BuildingUpgradePanel
    {
        void OnEnable() => mainBuilding.Setup(playerHolder.Player, DormManager.Instance.Buildings.Kitchen);
    }
}