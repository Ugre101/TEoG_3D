using DormAndHome.Dorm;

namespace Safe_To_Share.Scripts.GameUIAndMenus.DormUI.UI
{
    public sealed class KitchenUpgradePanel : BuildingUpgradePanel
    {
        void OnEnable() => mainBuilding.Setup(playerHolder.Player, DormManager.Instance.Buildings.Kitchen);
    }
}