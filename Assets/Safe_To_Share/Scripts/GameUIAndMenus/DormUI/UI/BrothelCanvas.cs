namespace Safe_To_Share.Scripts.GameUIAndMenus.DormUI.UI
{
    public sealed class BrothelCanvas : DormBuildingCanvas
    {
        protected override bool HasBuilding => Manager.Buildings.VillageBuildings.Brothel.Level > 0;
    }
}