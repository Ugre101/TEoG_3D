namespace DormAndHome.Dorm.UI
{
    public class BrothelCanvas : DormBuildingCanvas
    {
        protected override bool HasBuilding => Manager.Buildings.VillageBuildings.Brothel.Level > 0;
    }
}