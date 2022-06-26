using DormAndHome.Dorm.Buildings;

namespace DormAndHome.Dorm.UI
{
    public class BrothelOptionPanel : BuildingOptionPanel
    {
        static DormBrothel Brothel => DormManager.Instance.Buildings.VillageBuildings.Brothel;

        public override void Setup() => dropdown.SetupTmpDropDown(Brothel.Setting, ChangeSettings);

        static void ChangeSettings(int arg0) =>
            Brothel.Setting = UgreTools.IntToEnum(arg0, DormBrothel.BrothelSettings.Closed);
    }
}