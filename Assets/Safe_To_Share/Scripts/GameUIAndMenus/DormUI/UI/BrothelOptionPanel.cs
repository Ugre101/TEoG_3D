using DormAndHome.Dorm;
using DormAndHome.Dorm.Buildings;
using Safe_To_Share.Scripts.Static;

namespace Safe_To_Share.Scripts.GameUIAndMenus.DormUI.UI {
    public sealed class BrothelOptionPanel : BuildingOptionPanel {
        static DormBrothel Brothel => DormManager.Instance.Buildings.VillageBuildings.Brothel;

        public override void Setup() => dropdown.SetupTmpDropDown(Brothel.Setting, ChangeSettings);

        static void ChangeSettings(int arg0) =>
            Brothel.Setting = UgreTools.IntToEnum(arg0, DormBrothel.BrothelSettings.Closed);
    }
}