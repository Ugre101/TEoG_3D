using DormAndHome.Dorm;

namespace Safe_To_Share.Scripts.GameUIAndMenus.DormUI.UI
{
    public class BrothelUpgradePanel : BuildingUpgradePanel
    {
        void OnEnable() =>
            mainBuilding.Setup(playerHolder.Player, DormManager.Instance.Buildings.VillageBuildings.Brothel);
    }
}