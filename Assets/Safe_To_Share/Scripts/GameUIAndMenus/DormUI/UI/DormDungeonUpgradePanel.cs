using DormAndHome.Dorm;
using Safe_To_Share.Scripts.Static;

namespace Safe_To_Share.Scripts.GameUIAndMenus.DormUI.UI
{
    public sealed class DormDungeonUpgradePanel : BuildingUpgradePanel
    {
        void OnEnable()
        {
            mainBuilding.Setup(playerHolder.Player, DormManager.Instance.Buildings.Dungeon);
            ShowSubOptions();
            DormUpgradeButton.UpdateDormBuildings += ShowSubOptions;
        }

        void OnDisable() => DormUpgradeButton.UpdateDormBuildings -= ShowSubOptions;

        void ShowSubOptions()
        {
            content.KillChildren();
            if (DormManager.Instance.Buildings.Dungeon.Level <= 1)
                return;

            //   const string stoneDesc = "A magic stone enabled slow essene growth among you followers by converting the natural essence floating in the air.";
            //   Instantiate(prefab,content).SubSetup(playerHolder.Player,DormManager.Instance.Buildings.DormLodge.EssenceStone,"Essence Stone",stoneDesc);
        }
    }
}