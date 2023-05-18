namespace Safe_To_Share.Scripts.GameUIAndMenus.DormUI.UI
{
    public class BrothelCanvas : DormBuildingCanvas
    {
        protected override bool HasBuilding => Manager.Buildings.VillageBuildings.Brothel.Level > 0;
    }
}